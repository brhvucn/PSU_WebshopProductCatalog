#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Context Manager - discover, load, and manage project context on demand.
    Requires PowerShell Core (pwsh 7+) for cross-platform support.

.DESCRIPTION
    Prevents loading all context at once. Instead, discover what is relevant,
    then load only what the current task needs.

    Core principle: Do not load all context. First discover, then load selectively.

.PARAMETER Command
    Operation to execute: discover, load, tree, groups, info, search, keywords, harvest, compress, organize

.PARAMETER Task
    Task description used for discovery matching (free-text).

.PARAMETER Keyword
    Alternative: specific keyword(s) to search for (comma-separated).

.PARAMETER Path
    One or more relative file paths to load (e.g., "context/architecture/cqrs.md").

.PARAMETER Group
    Predefined context group name: cqrs, database, api, frontend, templates, examples, patterns, packages, all

.PARAMETER Source
    Source path or raw text for harvest operation.
    For harvest: path to a file, or a string of raw notes/text to classify and store.
    For compress: path to the file to compress.

.PARAMETER Target
    Target category for harvest operation (architecture, patterns, packages, examples, frontend, techstack).
    If omitted, harvest will auto-detect the best category.

.PARAMETER MaxTokens
    Token budget for load/discover. On load: stop adding files once budget exceeded.
    On discover: only show files fitting within budget (default: 20000).

.PARAMETER MaxFiles
    Maximum number of files to return (discover only). 0 = no limit.

.PARAMETER Preview
    Show what would be loaded without actually loading content (dry-run).
    For compress: show what would be removed without writing changes.

.PARAMETER FullScan
    Scan full file body even when metadata matches. Used when metadata-only
    search misses relevant files. Still respects -MaxTokens and -MaxFiles.

.PARAMETER Json
    Output machine-readable JSON (for discover, tree, info).

.PARAMETER Help
    Show this help message.

.EXAMPLE
    # Discover context relevant to a task
    .opencode\scripts\context-manager.ps1 discover -Task "Create ticket query handler"

    # Discover using keywords
    .opencode\scripts\context-manager.ps1 discover -Keyword "cqrs,command,handler"

    # A minimal loading session:
    .opencode\scripts\context-manager.ps1 discover -Task "..."  # step 1
    .opencode\scripts\context-manager.ps1 load -Path "context/architecture/cqrs.md","context/patterns/command-handler-template.md"  # step 2

    # Load a predefined group
    .opencode\scripts\context-manager.ps1 load -Group cqrs -MaxTokens 4000

    # Preview what a group load would cost (dry-run)
    .opencode\scripts\context-manager.ps1 load -Group cqrs -MaxTokens 4000 -Preview

    # Discover with file limit
    .opencode\scripts\context-manager.ps1 discover -Task "cqrs" -MaxFiles 5

    # Discover within token budget
    .opencode\scripts\context-manager.ps1 discover -Keyword "api" -MaxTokens 3000

    # Emergency full scan (scan file body even if metadata matches)
    .opencode\scripts\context-manager.ps1 discover -Task "unusual term" -FullScan -MaxTokens 4000

    # Show context hierarchy tree
    .opencode\scripts\context-manager.ps1 tree

    # Show info about a specific file
    .opencode\scripts\context-manager.ps1 info -Path "context/patterns/repository-template.md"

    # List all unique metadata keywords (when you don't know what to search for)
    .opencode\scripts\context-manager.ps1 keywords

    # Compress a context file (remove redundancy, keep rules/examples)
    .opencode\scripts\context-manager.ps1 compress -Path "context/patterns/command-handler-template.md"
    .opencode\scripts\context-manager.ps1 compress -Path "context/patterns/command-handler-template.md" -Preview

    # Harvest raw notes or a file into the correct context category
    .opencode\scripts\context-manager.ps1 harvest -Source "notes.md" -Target patterns
    .opencode\scripts\context-manager.ps1 harvest -Source "Always use sealed classes for handlers." -Target patterns

    # Reload groups from groups.json (after editing the config)
    .opencode\scripts\context-manager.ps1 groups
#>

param(
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateSet("discover", "load", "tree", "groups", "info", "search", "keywords", "harvest", "compress", "organize", "promote", "stale")]
    [string]$Command,

    [Parameter()]
    [string]$Task = "",

    [Parameter()]
    [string]$Keyword = "",

    [Parameter()]
    [string[]]$Path = @(),

    [Parameter()]
    [string]$Group = "",

    [Parameter()]
    [string]$Source = "",

    [Parameter()]
    [ValidateSet("", "architecture", "patterns", "packages", "examples", "frontend", "techstack")]
    [string]$Target = "",

    [Parameter()]
    [int]$MaxTokens = 20000,

    [Parameter()]
    [int]$MaxFiles = 0,

    [Parameter()]
    [switch]$Preview,

    [Parameter()]
    [switch]$FullScan,

    [Parameter()]
    [switch]$Json,

    [Parameter()]
    [switch]$Help
)

# ------------------------------------------------------------------
# CONSTANTS & PATHS
# ------------------------------------------------------------------

$ScriptRoot = $PSScriptRoot
$ProjectRoot = (Get-Item $ScriptRoot).Parent.Parent.FullName

# Search paths and harvest targets are loaded from groups.json (_search / _harvest sections).
# $SearchPaths and $HarvestTargets are populated by Load-ContextGroups below.
$SearchPaths   = @()
$HarvestTargets = @{}

# ------------------------------------------------------------------
# HELPER FUNCTIONS
# ------------------------------------------------------------------

function Get-RelativePath {
    param([string]$FullPath)
    $rel = $FullPath.Substring($ProjectRoot.Length).TrimStart('\', '/')
    return $rel.Replace('\', '/')
}

function Get-FileMetadata {
    param([string]$FullPath)
    $rel = Get-RelativePath $FullPath
    $content = Get-Content $FullPath -Raw -ErrorAction SilentlyContinue
    $lines = ($content -split "`r`n|`n").Count
    if ($content -eq "") { $lines = 0 }
    $bytes = (Get-Item $FullPath).Length
    $estTokens = [math]::Max(1, [math]::Round($bytes / 4))
    $depth = ($rel -split '/').Count - 1

    return @{
        RelPath    = $rel
        Lines      = $lines
        Bytes      = $bytes
        EstTokens  = $estTokens
        Depth      = $depth
        Name       = Split-Path $rel -Leaf
        Category   = ($rel -split '/')[0..1] -join '/'
    }
}

function Get-YamlMetadata {
    <#
    .SYNOPSIS
        Parses YAML frontmatter from a markdown file.
        Returns hashtable with domain, capabilities, keywords, priority, cost.
    #>
    param([string]$FullPath)
    $content = Get-Content $FullPath -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return @{} }

    $result = @{}

    # Normalize line endings to \n for consistent regex matching
    $normalized = $content -replace "`r`n", "`n" -replace "`r", "`n"

    # Match YAML frontmatter between --- markers
    if ($normalized -match '(?s)^---\s*\n(.*?)\n---') {
        $yaml = $matches[1]

        # Split by newline for list parsing
        $yamlLines = $yaml -split "`n"

        # Extract domain
        if ($yaml -match '(?m)^domain:\s*(.+)$') {
            $result.Domain = $matches[1].Trim()
        }

        # Extract capabilities list
        $caps = @()
        $inCaps = $false
        foreach ($line in $yamlLines) {
            if ($line -match '^capabilities:') { $inCaps = $true; continue }
            if ($inCaps -and $line -match '^\s*-\s+(.+)$') { $caps += $matches[1].Trim() }
            if ($inCaps -and $line -match '^\w+:' -and $line -notmatch '^\s') { $inCaps = $false }
        }
        if ($caps.Count -gt 0) { $result.Capabilities = $caps }

        # Extract keywords list
        $kws = @()
        $inKw = $false
        foreach ($line in $yamlLines) {
            if ($line -match '^keywords:') { $inKw = $true; continue }
            if ($inKw -and $line -match '^\s*-\s+(.+)$') { $kws += $matches[1].Trim() }
            if ($inKw -and $line -match '^\w+:' -and $line -notmatch '^\s') { $inKw = $false }
        }
        if ($kws.Count -gt 0) { $result.Keywords = $kws }

        # Extract priority
        if ($yaml -match '(?m)^priority:\s*(.+)$') {
            $result.Priority = $matches[1].Trim().ToLower()
        }

        # Extract cost
        if ($yaml -match '(?m)^cost:\s*(.+)$') {
            $result.Cost = $matches[1].Trim().ToLower()
        }
    }

    return $result
}

function Get-AllContextFiles {
    $files = @()
    foreach ($relPath in $SearchPaths) {
        $fullDir = Join-Path $ProjectRoot $relPath
        if (Test-Path $fullDir) {
            Get-ChildItem -Path $fullDir -Recurse -Filter *.md -File | Where-Object {
                # Exclude README files — they are human-readable guides, not context files
                $_.Name -ne 'README.md'
            } | ForEach-Object {
                $files += $_.FullName
            }
        }
    }
    return $files
}

function Write-Heading {
    param([string]$Text, [string]$Color = "Cyan")
    Write-Host "`n$Text" -ForegroundColor $Color
    $line = ""
    for ($i = 0; $i -lt [math]::Min($Text.Length, 80); $i++) { $line += "-" }
    Write-Host $line -ForegroundColor $Color
}

function Format-Bytes {
    param([int]$Bytes)
    if ($Bytes -ge 1024) { return "$([math]::Round($Bytes/1024, 1)) KB" }
    return "$Bytes B"
}

function Write-TokenWarning {
    param([int]$Tokens, [int]$Threshold = $MaxTokens)
    if ($Tokens -gt $Threshold) {
        Write-Host "  [!] WARNING: ~$Tokens tokens exceeds threshold of $Threshold" -ForegroundColor Yellow
        Write-Host "  Consider loading fewer files or using more specific targets." -ForegroundColor Yellow
    } elseif ($Tokens -gt ($Threshold * 0.8)) {
        Write-Host "  [!] Approaching threshold: ~$Tokens / $Threshold tokens" -ForegroundColor DarkYellow
    } else {
        Write-Host "  Token estimate: ~$Tokens" -ForegroundColor Green
    }
}

# ------------------------------------------------------------------
# GROUP RESOLVER (no hardcoded file lists)
# ------------------------------------------------------------------

function Resolve-GroupFiles {
    <#
    .SYNOPSIS
        Resolves a group definition to actual file paths dynamically.
        No hardcoded file lists - uses directory globbing or keyword scoring.

    .PARAMETER GroupInfo
        Hashtable with Type, Paths/Keywords/SearchDirs, MaxFiles.
    #>
    param($GroupInfo)

    $files = @()

    switch ($GroupInfo.Type) {
        "directory" {
            foreach ($dir in $GroupInfo.Paths) {
                $fullDir = Join-Path $ProjectRoot $dir
                if (Test-Path $fullDir) {
                    $files += Get-ChildItem -Path $fullDir -Recurse -Filter *.md -File -ErrorAction SilentlyContinue |
                        ForEach-Object { $_.FullName }
                }
            }
        }

        "keyword" {
            $searchDirs = @(".opencode/context")
            if ($GroupInfo.SearchDirs) { $searchDirs = $GroupInfo.SearchDirs }
            $candidates = @()
            foreach ($dir in $searchDirs) {
                $fullDir = Join-Path $ProjectRoot $dir
                if (Test-Path $fullDir) {
                    $candidates += Get-ChildItem -Path $fullDir -Recurse -Filter *.md -File -ErrorAction SilentlyContinue |
                        ForEach-Object { $_.FullName }
                }
            }
            $candidates = $candidates | Select-Object -Unique

            $scored = @()
            foreach ($file in $candidates) {
                $rel = Get-RelativePath $file
                $yaml = Get-YamlMetadata $file
                $baseName = (Get-Item $file).BaseName.ToLower()
                $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
                $contentLower = if ($content) { $content.ToLower() } else { "" }

                $score = 0
                foreach ($kw in $GroupInfo.Keywords) {
                    $kwLower = $kw.ToLower()

                    # Metadata capability match (highest)
                    $matched = $false
                    if ($yaml.Capabilities) {
                        foreach ($cap in $yaml.Capabilities) {
                            if ($cap.ToLower() -match [regex]::Escape($kwLower)) {
                                $score += 15
                                $matched = $true
                                break
                            }
                        }
                    }

                    # Metadata keyword match
                    if (-not $matched -and $yaml.Keywords) {
                        foreach ($ykw in $yaml.Keywords) {
                            if ($ykw.ToLower() -match [regex]::Escape($kwLower)) {
                                $score += 8
                                $matched = $true
                                break
                            }
                        }
                    }

                    # Filename match
                    if ($baseName -match [regex]::Escape($kwLower)) {
                        $score += 10
                    } elseif ($rel.ToLower() -match [regex]::Escape($kwLower)) {
                        $score += 5
                    } elseif (-not $matched -and $contentLower -match [regex]::Escape($kwLower)) {
                        $score += 3
                    }
                }

                if ($score -gt 0) {
                    # Apply priority modifier
                    $modifier = 1.0
                    if ($yaml.Priority -eq 'high') { $modifier = 1.2 }
                    elseif ($yaml.Priority -eq 'low') { $modifier = 0.8 }
                    if ($yaml.Cost -eq 'high') { $modifier *= 0.8 }

                    $finalScore = [math]::Round($score * $modifier)
                    $scored += @{ File = $file; Score = $finalScore; RelPath = $rel }
                }
            }

            $topN = 10
            if ($GroupInfo.MaxFiles) { $topN = $GroupInfo.MaxFiles }
            $files = $scored | Sort-Object Score -Descending | Select-Object -First $topN |
                ForEach-Object { $_.File }
        }

        "file" {
            foreach ($path in $GroupInfo.Paths) {
                $fullPath = Join-Path $ProjectRoot $path
                if (Test-Path $fullPath) {
                    $files += $fullPath
                }
            }
        }
    }

    return $files | Select-Object -Unique
}

# ------------------------------------------------------------------
# CONTEXT GROUPS - loaded from groups.json (configurable)
# ------------------------------------------------------------------
#
# Groups are defined in .opencode/scripts/groups.json - NOT hardcoded here.
# Edit groups.json to add, remove or change groups without touching this script.
#
# Three group types:
#   type = "directory"  -> glob all .md files in Paths. Add files = auto-included.
#   type = "keyword"    -> score files by keyword relevance, take top MaxFiles.
#   type = "file"       -> load a single hardcoded path (only for essentials).

function Load-ContextGroups {
    $groupsFile = Join-Path $ScriptRoot "groups.json"
    if (-not (Test-Path $groupsFile)) {
        Write-Host "[!] groups.json not found at: $groupsFile" -ForegroundColor Red
        Write-Host "    Create it or restore from the blueprint." -ForegroundColor Gray
        return [ordered]@{}
    }

    try {
        $raw    = Get-Content $groupsFile -Raw -Encoding UTF8
        $parsed = $raw | ConvertFrom-Json

        # --- Populate global search paths from _search section ---
        if ($parsed._search -and $parsed._search.paths) {
            $script:SearchPaths = @($parsed._search.paths)
        }
        else {
            Write-Host "[!] groups.json is missing '_search.paths' - defaulting to .opencode/context" -ForegroundColor Yellow
            $script:SearchPaths = @(".opencode/context")
        }

        # --- Populate global harvest targets from _harvest section ---
        if ($parsed._harvest) {
            foreach ($prop in $parsed._harvest.PSObject.Properties) {
                if ($prop.Name -notlike '_*') {
                    $script:HarvestTargets[$prop.Name] = $prop.Value
                }
            }
        }

        # --- Load named groups (skip _ prefixed config sections) ---
        $groups = [ordered]@{}
        foreach ($prop in $parsed.PSObject.Properties) {
            $name = $prop.Name
            if ($name -like '_*') { continue }   # skip _search, _harvest, _comment

            $val = $prop.Value
            $entry = @{
                Description = if ($val.description) { $val.description } else { $name }
                Type        = if ($val.type)        { $val.type }        else { "directory" }
            }

            if ($val.keywords)   { $entry.Keywords   = @($val.keywords) }
            if ($val.paths)      { $entry.Paths       = @($val.paths) }
            if ($val.searchDirs) { $entry.SearchDirs  = @($val.searchDirs) }
            if ($val.maxFiles)   { $entry.MaxFiles    = [int]$val.maxFiles }

            $groups[$name] = $entry
        }
        return $groups
    }
    catch {
        Write-Host "[!] Failed to parse groups.json: $_" -ForegroundColor Red
        return [ordered]@{}
    }
}

$ContextGroups = Load-ContextGroups

# ------------------------------------------------------------------
# COMMAND: DISCOVER
# ------------------------------------------------------------------

function Invoke-Discover {
    $keywords = @()
    if ($Task) {
        $keywords = ($Task -split '\s+') | Where-Object { $_.Length -gt 2 } | ForEach-Object { $_.Trim(',.?!:;') }
    }
    if ($Keyword) {
        $keywords += ($Keyword -split ',' | ForEach-Object { $_.Trim() })
    }
    $keywords = $keywords | Where-Object { $_ -ne '' } | Select-Object -Unique

    if ($keywords.Count -eq 0) {
        Write-Host "No keywords provided. Use -Task or -Keyword." -ForegroundColor Red
        return
    }

    Write-Heading "Context Discovery"
    $scanMode = if ($FullScan) { " [FULL SCAN]" } else { "" }
    Write-Host "Searching for: '$($keywords -join "', '")'$scanMode" -ForegroundColor Gray
    Write-Host ""

    $allFiles = Get-AllContextFiles
    $results = @()

    foreach ($file in $allFiles) {
        $rel = Get-RelativePath $file
        $meta = Get-FileMetadata $file
        $yaml = Get-YamlMetadata $file
        $baseName = (Get-Item $file).BaseName.ToLower()
        $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
        $contentLower = if ($content) { $content.ToLower() } else { "" }

        $matchScore = 0
        $matchedKeywords = @()

        foreach ($kw in $keywords) {
            $kwLower = $kw.ToLower()

            # Metadata capability match (highest priority)
            $capMatch = $false
            if ($yaml.Capabilities) {
                foreach ($cap in $yaml.Capabilities) {
                    if ($cap.ToLower() -match [regex]::Escape($kwLower)) {
                        $matchScore += 15
                        $matchedKeywords += "$kw(capability)"
                        $capMatch = $true
                        break
                    }
                }
            }

            # Metadata keyword match
            $kwMatch = $false
            if ($yaml.Keywords) {
                foreach ($ykw in $yaml.Keywords) {
                    if ($ykw.ToLower() -match [regex]::Escape($kwLower)) {
                        $matchScore += 8
                        $matchedKeywords += "$kw(meta)"
                        $kwMatch = $true
                        break
                    }
                }
            }

            # Filename match
            $nameMatch = $false
            if ($baseName -match [regex]::Escape($kwLower)) {
                $matchScore += 10
                $matchedKeywords += "$kw(filename)"
                $nameMatch = $true
            }

            # Path match (only if no filename match)
            $pathMatch = $false
            if (-not $nameMatch -and $rel.ToLower() -match [regex]::Escape($kwLower)) {
                $matchScore += 5
                $matchedKeywords += "$kw(path)"
                $pathMatch = $true
            }

            # Content match: always if FullScan; otherwise only if no metadata/filename/path match
            $scanContent = $FullScan -or (-not $capMatch -and -not $kwMatch -and -not $nameMatch -and -not $pathMatch)
            if ($scanContent -and $contentLower -match [regex]::Escape($kwLower)) {
                $matchScore += 3
                if (-not $capMatch -and -not $kwMatch -and -not $nameMatch -and -not $pathMatch) {
                    $matchedKeywords += $kw
                } else {
                    $matchedKeywords += "$kw(body)"
                }
            }
        }

        if ($matchScore -gt 0) {
            # Apply priority modifier
            $modifier = 1.0
            if ($yaml.Priority -eq 'high') { $modifier = 1.2 }
            elseif ($yaml.Priority -eq 'low') { $modifier = 0.8 }
            # Apply cost penalty
            if ($yaml.Cost -eq 'high') { $modifier *= 0.8 }

            $finalScore = [math]::Round($matchScore * $modifier)

            $results += [PSCustomObject]@{
                RelPath         = $rel
                Lines           = $meta.Lines
                EstTokens       = $meta.EstTokens
                Depth           = $meta.Depth
                Score           = $finalScore
                RawScore        = $matchScore
                MatchedKeywords = $matchedKeywords -join ", "
                Category        = $meta.Category
                Domain          = if ($yaml.Domain) { $yaml.Domain } else { "" }
            }
        }
    }

    if ($results.Count -eq 0) {
        Write-Host "No matching context found for keywords: $($keywords -join ', ')" -ForegroundColor Yellow
        Write-Host "Try broader terms or check .opencode/context/ for available files." -ForegroundColor Gray
        return
    }

    $sorted = $results | Sort-Object Score -Descending

    # --- apply MaxFiles and MaxTokens filters ---
    $isBudgeted = ($MaxTokens -gt 0 -and $MaxTokens -lt 100000)
    $displayResults = $sorted
    $budgetSkipped = 0

    if ($isBudgeted) {
        $budgetFiles = @()
        $tokenSum = 0
        foreach ($r in $sorted) {
            if ($tokenSum + $r.EstTokens -gt $MaxTokens) {
                $budgetSkipped++
                continue
            }
            $budgetFiles += $r
            $tokenSum += $r.EstTokens
        }
        $displayResults = $budgetFiles
    }

    if ($MaxFiles -gt 0 -and $MaxFiles -lt $displayResults.Count) {
        $displayResults = $displayResults | Select-Object -First $MaxFiles
    }

    if ($Json) {
        return $displayResults | ConvertTo-Json
    }

    $shownCount = $displayResults.Count
    $totalFound = $results.Count
    $totalTokens = ($displayResults | Measure-Object -Property EstTokens -Sum).Sum

    Write-Host ("{0,-6} {1,-50} {2,6} {3,7} {4,5}" -f "Score", "File", "Lines", "Tokens", "Depth") -ForegroundColor Cyan
    Write-Host ("-" * 80) -ForegroundColor DarkGray

    $displayResults | ForEach-Object {
        $color = if ($_.Score -ge 10) { "Green" } elseif ($_.Score -ge 5) { "White" } else { "Gray" }
        Write-Host ("{0,4}   {1,-50} {2,6} {3,7} {4,5}" -f $_.Score, $_.RelPath, $_.Lines, $_.EstTokens, $_.Depth) -ForegroundColor $color
    }

    Write-Host ""
    if ($shownCount -lt $totalFound) {
        Write-Host "Showing $shownCount of $totalFound relevant files" -ForegroundColor Green
        if ($isBudgeted) {
            Write-Host "  ~$totalTokens / $MaxTokens tokens used  ($budgetSkipped files excluded by budget)" -ForegroundColor DarkGray
        }
        if ($MaxFiles -gt 0 -and $MaxFiles -lt $totalFound) {
            Write-Host "  (limited to top $MaxFiles by -MaxFiles)" -ForegroundColor DarkGray
        }
        Write-Host "  Increase -MaxTokens or -MaxFiles to see more." -ForegroundColor Gray
    } else {
        Write-Host "Found $totalFound relevant files (~$totalTokens tokens)" -ForegroundColor Green
    }
    Write-Host ""
    Write-Host "Recommended: Load the top files using:" -ForegroundColor Gray
    $topN = [math]::Min(4, $displayResults.Count)
    $topFiles = $displayResults | Select-Object -First $topN
    $paths = ($topFiles.RelPath -join '","')
    Write-Host "  .opencode\scripts\context-manager.ps1 load -Path `"$paths`"" -ForegroundColor Yellow
    if ($isBudgeted -and $budgetSkipped -gt 0) {
        Write-Host "  With higher budget: -MaxTokens $([math]::Round(($totalTokens + 2000) / 1000) * 1000)" -ForegroundColor Yellow
    }
}

# ------------------------------------------------------------------
# COMMAND: LOAD
# ------------------------------------------------------------------

function Invoke-Load {
    $filesToLoad = @()
    $budget = $MaxTokens
    $isBudgeted = ($budget -gt 0 -and $budget -lt 100000)

    # --- resolve group files ---
    if ($Group) {
        if ($ContextGroups.Contains($Group)) {
            $groupInfo = $ContextGroups[$Group]
            Write-Heading "Loading Group: $Group"
            Write-Host "$($groupInfo.Description)" -ForegroundColor Gray
            Write-Host ""

            $groupType = if ($groupInfo.Type) { $groupInfo.Type } else { "directory" }
            $filesToLoad = Resolve-GroupFiles $groupInfo

            if ($groupType -eq "keyword") {
                Write-Host "  (auto-discovered via keywords: $($groupInfo.Keywords -join ', '))" -ForegroundColor DarkGray
            }
            Write-Host "  Found $($filesToLoad.Count) candidate files." -ForegroundColor Gray
            if ($isBudgeted) {
                Write-Host "  Budget: ~$budget tokens  (use -MaxTokens to adjust)" -ForegroundColor DarkGray
            }
            Write-Host ""
        } else {
            Write-Host "Unknown group: '$Group'. Use 'groups' command to see available groups." -ForegroundColor Red
            return
        }
    }

    # --- resolve explicit paths ---
    foreach ($p in $Path) {
        $fullPath = Join-Path $ProjectRoot $p
        if (Test-Path $fullPath) {
            $filesToLoad += $fullPath
        } else {
            $altPath = Join-Path $ProjectRoot ".opencode/$p"
            if (Test-Path $altPath) {
                $filesToLoad += $altPath
            } else {
                $matched = Get-ChildItem -Path $fullPath -File -ErrorAction SilentlyContinue
                if ($matched) {
                    $filesToLoad += $matched.FullName
                } else {
                    Write-Host "File not found: $p" -ForegroundColor Red
                }
            }
        }
    }

    if ($filesToLoad.Count -eq 0) {
        Write-Host "No files to load. Use -Path <relative-path> or -Group <group-name>." -ForegroundColor Yellow
        Write-Host "Example: .opencode\scripts\context-manager.ps1 load -Path context/architecture/cqrs.md"
        return
    }

    $filesToLoad = $filesToLoad | Select-Object -Unique

    # --- preview mode: show plan without loading content ---
    if ($Preview) {
        Write-Heading "Preview (dry-run)"
        Write-Host ""

        $totalTokens = 0
        $totalLines = 0
        $i = 0
        $stoppedEarly = $false

        foreach ($file in $filesToLoad) {
            if (-not (Test-Path $file)) { continue }
            $meta = Get-FileMetadata $file

            if ($isBudgeted -and ($totalTokens + $meta.EstTokens) -gt $budget) {
                Write-Host "  ... stopped at $i files (~$totalTokens tokens) - budget of ~$budget exhausted" -ForegroundColor DarkYellow
                $stoppedEarly = $true
                break
            }

            $i++
            $totalTokens += $meta.EstTokens
            $totalLines += $meta.Lines
            Write-Host ("  {0,2}. {1,-55} ~{2,5} tok  ({3,4} lines, depth {4})" -f $i, $meta.RelPath, $meta.EstTokens, $meta.Lines, $meta.Depth) -ForegroundColor Gray
        }

        Write-Host ""
        Write-Host ("-" * 50) -ForegroundColor DarkGray
        Write-Host "Preview summary:" -ForegroundColor White
        Write-Host "  Would load: $i files" -ForegroundColor Gray
        Write-Host "  Total lines:  $totalLines" -ForegroundColor Gray
        Write-Host "  Total tokens: ~$totalTokens" -ForegroundColor Gray
        if ($isBudgeted) { Write-Host "  Budget:       ~$budget tokens" -ForegroundColor DarkGray }
        if ($stoppedEarly) { Write-Host "  [!] Budget exhausted - $($filesToLoad.Count - $i) files skipped" -ForegroundColor Yellow }

        Write-Host ""
        Write-Host "To load:" -ForegroundColor Cyan
        Write-Host "  Remove -Preview to load these files" -ForegroundColor Gray
        if ($stoppedEarly) {
            Write-Host "  Increase budget: -MaxTokens $([math]::Round($totalTokens * 1.5))" -ForegroundColor Gray
        }
        return
    }

    # --- normal load mode ---
    Write-Heading "Loading Context"
    Write-Host ""

    $totalTokens = 0
    $totalLines = 0
    $loadedMeta = @()
    $skippedCount = 0

    foreach ($file in $filesToLoad) {
        if (-not (Test-Path $file)) { continue }
        $meta = Get-FileMetadata $file

        # Respect token budget
        if ($isBudgeted -and ($totalTokens + $meta.EstTokens) -gt $budget) {
            $skippedCount++
            continue
        }

        $totalTokens += $meta.EstTokens
        $totalLines += $meta.Lines
        $loadedMeta += $meta

        Write-Host "=> $($meta.RelPath)" -ForegroundColor White
        Write-Host "   Lines: $($meta.Lines) | Tokens: ~$($meta.EstTokens) | Depth: $($meta.Depth)" -ForegroundColor DarkGray

        $content = Get-Content $file -Raw
        if ($content) {
            $lines = $content -split "`r`n|`n"
            $previewLines = $lines | Select-Object -First ([math]::Min(3, $lines.Count))
            foreach ($line in $previewLines) {
                if ($line -match '^# ') {
                    Write-Host "   $line" -ForegroundColor Cyan
                } elseif ($line -match '^## ') {
                    Write-Host "   $line" -ForegroundColor Green
                }
            }
            if ($lines.Count -gt 3) {
                Write-Host "   ... ($($lines.Count - 3) more lines)" -ForegroundColor DarkGray
            }
        }
        Write-Host ""
    }

    Write-Host ("-" * 50) -ForegroundColor DarkGray
    Write-Host "Summary:" -ForegroundColor White
    Write-Host "  Files loaded: $($loadedMeta.Count)" -ForegroundColor Gray
    if ($skippedCount -gt 0) {
        Write-Host "  Files skipped: $skippedCount (budget of ~$budget exhausted)" -ForegroundColor Yellow
    }
    Write-Host "  Total lines:  $totalLines" -ForegroundColor Gray
    Write-TokenWarning -Tokens $totalTokens

    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Review the loaded context above" -ForegroundColor Gray
    Write-Host "  2. Start implementation with this context in mind" -ForegroundColor Gray
    Write-Host "  3. Use 'load -Path ...' to load more if needed" -ForegroundColor Gray
    Write-Host "  4. Use 'discover -Task ...' to find additional context" -ForegroundColor Gray
    if ($skippedCount -gt 0) {
        Write-Host "  5. Increase budget: -MaxTokens $([math]::Round(($totalTokens + 2000) / 1000) * 1000)" -ForegroundColor Gray
    }
}

# ------------------------------------------------------------------
# COMMAND: TREE
# ------------------------------------------------------------------

function Invoke-Tree {
    Write-Heading "Context Hierarchy Tree"

    $allFiles = Get-AllContextFiles | ForEach-Object { Get-FileMetadata $_ } | Sort-Object RelPath

    if ($allFiles.Count -eq 0) {
        Write-Host "  No context files found." -ForegroundColor Yellow
        return
    }

    # Group by top-level category
    $grouped = $allFiles | Group-Object { ($_.RelPath -split '/')[0..1] -join '/' }

    $totalFiles = 0
    $totalTokens = 0

    foreach ($g in $grouped) {
        $catLabel = $g.Name
        Write-Host ""
        Write-Host "  [+] $catLabel ($($g.Count) files)" -ForegroundColor Cyan

        foreach ($meta in $g.Group) {
            $totalFiles++
            $totalTokens += $meta.EstTokens

            $parts = $meta.RelPath -split '/'
            $depth = $parts.Count - 1
            $indent = "  " + ("    " * $depth)
            $prefix = if ($depth -gt 1) { "  \-- " } else { "  +-- " }
            $tokStr = "~$($meta.EstTokens) tok | $($meta.Lines) lines"

            Write-Host "$indent$prefix$($parts[-1]) ($tokStr)" -ForegroundColor Gray
        }
    }

    Write-Host ""
    $line = ""
    for ($i = 0; $i -lt 50; $i++) { $line += "-" }
    Write-Host $line -ForegroundColor DarkGray
    Write-Host "Total: $totalFiles files, ~$totalTokens tokens" -ForegroundColor White
    Write-Host ""
    Write-Host "Deepest hierarchies:" -ForegroundColor Cyan
    $deepest = $allFiles | Sort-Object Depth -Descending | Select-Object -First 5
    foreach ($d in $deepest) {
        Write-Host "  Level $($d.Depth): $($d.RelPath) ($($d.Lines) lines)" -ForegroundColor Gray
    }
    Write-Host ""
    Write-Host "Load on demand:" -ForegroundColor Green
    Write-Host "  Group load:  .opencode\scripts\context-manager.ps1 load -Group <name>" -ForegroundColor Gray
    Write-Host "  Single file: .opencode\scripts\context-manager.ps1 load -Path <path>" -ForegroundColor Gray
    Write-Host "  Groups:      .opencode\scripts\context-manager.ps1 groups" -ForegroundColor Gray

    if ($Json) {
        return $allFiles | ConvertTo-Json
    }
}

# ------------------------------------------------------------------
# COMMAND: GROUPS
# ------------------------------------------------------------------

function Invoke-Groups {
    Write-Heading "Available Context Groups"
    Write-Host "Use: .opencode\scripts\context-manager.ps1 load -Group <name>`n" -ForegroundColor Gray

    Write-Host ("{0,-14} {1,7} {2,10}  {3}" -f "Group", "Files", "Est.Tokens", "Description") -ForegroundColor Cyan
    $line = ""
    for ($i = 0; $i -lt 90; $i++) { $line += "-" }
    Write-Host $line -ForegroundColor DarkGray

    $totalAll = 0
    $maxGroupNameLen = ($ContextGroups.Keys | ForEach-Object { $_.Length } | Measure-Object -Maximum).Maximum
    $maxGroupNameLen = [math]::Max($maxGroupNameLen, 10)

    foreach ($grp in $ContextGroups.Keys) {
        $info = $ContextGroups[$grp]
        $groupFiles = Resolve-GroupFiles $info
        $fileCount = $groupFiles.Count
        $tokCount = 0
        foreach ($f in $groupFiles) {
            $tokCount += [math]::Max(1, [math]::Round((Get-Item $f -ErrorAction SilentlyContinue).Length / 4))
        }

        $color = if ($tokCount -gt 15000) { "Red" } elseif ($tokCount -gt 8000) { "Yellow" } else { "White" }
        $groupType = if ($info.Type) { $info.Type } else { "dir" }
        $typeTag = switch ($groupType) { "keyword" { "[kw]" } "directory" { "[dir]" } "file" { "[fi]" } default { "[?]" } }
        Write-Host ("{0,-14} {1,7} {2,10}  {3,-5} {4}" -f $grp, $fileCount, "~$tokCount", $typeTag, $info.Description) -ForegroundColor $color
        $totalAll += $tokCount
    }

    Write-Host $line -ForegroundColor DarkGray
    Write-Host ("{0,-14} {1,7} {2,10}" -f "TOTAL", "all", "~$totalAll") -ForegroundColor Cyan
    Write-Host ""
    Write-Host "[!] Groups marked in RED exceed 15K tokens - load specific files instead." -ForegroundColor DarkYellow
    Write-Host "[kw] = keyword-discovered  [dir] = directory-globbed  [fi] = single file" -ForegroundColor DarkGray
}

# ------------------------------------------------------------------
# COMMAND: INFO
# ------------------------------------------------------------------

function Invoke-Info {
    if ($Path.Count -eq 0) {
        Write-Host "Specify -Path to a context file." -ForegroundColor Red
        return
    }

    foreach ($p in $Path) {
        $fullPath = Join-Path $ProjectRoot $p
        if (-not (Test-Path $fullPath)) {
            $altPath = Join-Path $ProjectRoot ".opencode/$p"
            if (Test-Path $altPath) { $fullPath = $altPath }
            else {
                Write-Host "File not found: $p" -ForegroundColor Red
                continue
            }
        }

        $meta = Get-FileMetadata $fullPath

        Write-Heading "File Info: $($meta.RelPath)"
        Write-Host "  Name:      $($meta.Name)" -ForegroundColor Gray
        Write-Host "  Full path: $($meta.RelPath)" -ForegroundColor Gray
        Write-Host "  Lines:     $($meta.Lines)" -ForegroundColor Gray
        Write-Host "  Size:      $(Format-Bytes $meta.Bytes)" -ForegroundColor Gray
        Write-Host "  Est.Tokens: ~$($meta.EstTokens)" -ForegroundColor Gray
        Write-Host "  Depth:     Level $($meta.Depth) in hierarchy" -ForegroundColor Gray
        Write-Host "  Category:  $($meta.Category)" -ForegroundColor Gray

        $content = Get-Content $fullPath -Raw -ErrorAction SilentlyContinue
        if ($content) {
            $headings = @()
            foreach ($line in ($content -split "`r`n|`n")) {
                if ($line -match '^#{1,3}\s') { $headings += $line.Trim() }
            }
            if ($headings.Count -gt 0) {
                Write-Host "  Sections:" -ForegroundColor Cyan
                foreach ($h in $headings) { Write-Host "    $h" -ForegroundColor DarkGray }
            }
        }

        $rec = ""
        if ($meta.RelPath -like "*/architecture/*") { $rec = "Load with Group 'architecture' or specific related patterns" }
        elseif ($meta.RelPath -like "*/patterns/*") { $rec = "Load with Group 'patterns' or 'cqrs'/'database'/'api' depending on task" }
        elseif ($meta.RelPath -like "*/packages/*") { $rec = "Load with Group 'packages' or individually" }
        elseif ($meta.RelPath -like "*/examples/*") { $rec = "Load with Group 'examples' or 'frontend'" }
        elseif ($meta.RelPath -like "*/templates/*") { $rec = "Templates are for producing new artifacts, not for reading as context" }
        elseif ($meta.RelPath -like "*/frontend/*") { $rec = "Load with Group 'frontend' - expensive (~7K tokens)" }
        if ($rec) { Write-Host "  Recommendation: $rec" -ForegroundColor Green }

        Write-Host ""

        if ($Json) {
            return $meta | ConvertTo-Json
        }
    }
}

# ------------------------------------------------------------------
# COMMAND: SEARCH
# ------------------------------------------------------------------

function Invoke-Search {
    if (-not $Keyword -and -not $Task) {
        Write-Host "Use -Keyword or -Task to search across all context files." -ForegroundColor Red
        return
    }

    $searchTerms = @()
    if ($Keyword) { $searchTerms += ($Keyword -split ',' | ForEach-Object { $_.Trim() }) }
    if ($Task) { $searchTerms += ($Task -split '\s+' | Where-Object { $_.Length -gt 2 }) }
    $searchTerms = $searchTerms | Where-Object { $_ -ne '' } | Select-Object -Unique

    Write-Heading "Full-Text Search"
    Write-Host "Searching for: '$($searchTerms -join "', '")'" -ForegroundColor Gray
    Write-Host ""

    $allFiles = Get-AllContextFiles
    $hits = @()

    foreach ($file in $allFiles) {
        $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
        if (-not $content) { continue }
        $rel = Get-RelativePath $file

        foreach ($term in $searchTerms) {
            $pattern = [regex]::Escape($term)
            $matches = [regex]::Matches($content, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
            if ($matches.Count -gt 0) {
                $lines = $content -split "`r`n|`n"
                $contextLines = @()
                for ($i = 0; $i -lt $lines.Count; $i++) {
                    if ($lines[$i] -match $pattern -and $contextLines.Count -lt 3) {
                        $start = [math]::Max(0, $i - 1)
                        $end = [math]::Min($lines.Count - 1, $i + 1)
                        $snippet = ($lines[$start..$end] -join " ").Trim()
                        if ($snippet.Length -gt 120) { $snippet = $snippet.Substring(0, 117) + "..." }
                        $contextLines += "  line $($i+1): $snippet"
                    }
                }

                $hits += [PSCustomObject]@{
                    RelPath   = $rel
                    Term      = $term
                    Count     = $matches.Count
                    Context   = $contextLines -join "`n"
                }
            }
        }
    }

    if ($hits.Count -eq 0) {
        Write-Host "No matches found." -ForegroundColor Yellow
        return
    }

    $grouped = $hits | Group-Object RelPath
    Write-Host "Found $($hits.Count) matches across $($grouped.Count) files:`n" -ForegroundColor Green

    foreach ($g in $grouped) {
        Write-Host "  + $($g.Name)" -ForegroundColor Cyan
        Write-Host "     $($g.Count) total matches" -ForegroundColor DarkGray
        foreach ($match in $g.Group) {
            Write-Host "     -> $($match.Context)" -ForegroundColor Gray
        }
        Write-Host ""
    }
}

# ------------------------------------------------------------------
# COMMAND: KEYWORDS
# ------------------------------------------------------------------

function Invoke-Keywords {
    <#
    .SYNOPSIS
        Lists all unique metadata keywords and capabilities from context files.
        Useful when the agent does not know what search terms to use.
    #>
    $allFiles = Get-AllContextFiles
    $keywordMap = @{}   # keyword -> @{ domains = @(); files = @() }
    $capMap = @{}       # capability -> @{ domains = @(); files = @() }

    foreach ($file in $allFiles) {
        $yaml = Get-YamlMetadata $file
        $rel = Get-RelativePath $file
        $domain = if ($yaml.Domain) { $yaml.Domain } else { "unknown" }

        # Collect capabilities
        if ($yaml.Capabilities) {
            foreach ($cap in $yaml.Capabilities) {
                $key = $cap.ToLower()
                if (-not $capMap.ContainsKey($key)) {
                    $capMap[$key] = @{ Domains = @{}; Files = @() }
                }
                $capMap[$key].Domains[$domain] = $true
                if ($capMap[$key].Files.Count -lt 5) {
                    $capMap[$key].Files += $rel
                }
            }
        }

        # Collect keywords
        if ($yaml.Keywords) {
            foreach ($kw in $yaml.Keywords) {
                $key = $kw.ToLower()
                if (-not $keywordMap.ContainsKey($key)) {
                    $keywordMap[$key] = @{ Domains = @{}; Files = @() }
                }
                $keywordMap[$key].Domains[$domain] = $true
                if ($keywordMap[$key].Files.Count -lt 5) {
                    $keywordMap[$key].Files += $rel
                }
            }
        }
    }

    if ($Json) {
        return [PSCustomObject]@{
            Capabilities = ($capMap.Keys | Sort-Object)
            Keywords     = ($keywordMap.Keys | Sort-Object)
            FilesScanned = $allFiles.Count
        } | ConvertTo-Json
    }

    Write-Heading "Available Metadata Keywords"
    Write-Host "Scanned $($allFiles.Count) context files for YAML metadata."
    Write-Host ""

    # Print capabilities
    Write-Host "Capabilities (domæner):" -ForegroundColor Cyan
    Write-Host ("-" * 50) -ForegroundColor DarkGray
    $capMap.Keys | Sort-Object | ForEach-Object {
        $key = $_
        $info = $capMap[$key]
        $domains = ($info.Domains.Keys | Sort-Object) -join ", "
        Write-Host ("  {0,-25}  [{1}]" -f $key, $domains) -ForegroundColor White
    }

    Write-Host ""
    Write-Host "Keywords:" -ForegroundColor Cyan
    Write-Host ("-" * 50) -ForegroundColor DarkGray
    $keywordMap.Keys | Sort-Object | ForEach-Object {
        $key = $_
        $info = $keywordMap[$key]
        $domains = ($info.Domains.Keys | Sort-Object) -join ", "
        $sampleFiles = $info.Files -join ", "
        Write-Host ("  {0,-25}  [{1}]" -f $key, $domains) -ForegroundColor Gray
        Write-Host ("  {0,-25}  e.g. {1}" -f "", $sampleFiles) -ForegroundColor DarkGray
    }

    Write-Host ""
    Write-Host "Usage:" -ForegroundColor Cyan
    Write-Host "  .opencode\scripts\context-manager.ps1 discover -Keyword '<keyword>' -MaxTokens 4000" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Total: $($capMap.Count) capabilities, $($keywordMap.Count) keywords" -ForegroundColor Green
}

# ------------------------------------------------------------------
# COMMAND: COMPRESS
# ------------------------------------------------------------------
#
# Compress reduces token cost of a context file by removing redundancy,
# verbose explanations and old discussion - while preserving rules,
# examples, file paths, naming conventions and anti-patterns.
#
# Usage:
#   compress -Path "context/patterns/command-handler-template.md"
#   compress -Path "context/patterns/command-handler-template.md" -Preview
#
# With -Preview: shows a diff-style summary of what would be removed.
# Without -Preview: writes the compressed content back to the file (with backup).

function Invoke-Compress {
    if ($Path.Count -eq 0) {
        Write-Host 'Usage: compress -Path <relative-path-to-context-file>' -ForegroundColor Red
        Write-Host "Example: compress -Path `"context/patterns/command-handler-template.md`"" -ForegroundColor Gray
        return
    }

    $targetPath = $Path[0]
    $fullPath = Join-Path $ProjectRoot $targetPath
    if (-not (Test-Path $fullPath)) {
        $alt = Join-Path $ProjectRoot ".opencode/$targetPath"
        if (Test-Path $alt) { $fullPath = $alt; $targetPath = ".opencode/$targetPath" }
        else {
            Write-Host "File not found: $targetPath" -ForegroundColor Red
            return
        }
    }

    $original = Get-Content $fullPath -Raw -Encoding UTF8
    if (-not $original) {
        Write-Host "File is empty: $targetPath" -ForegroundColor Yellow
        return
    }

    $originalLines  = ($original -split "`r`n|`n")
    $originalTokens = [math]::Round($original.Length / 4)

    Write-Heading "Compress: $targetPath"
    Write-Host "  Original: $($originalLines.Count) lines, ~$originalTokens tokens" -ForegroundColor Gray
    Write-Host ""

    # ---- Compression rules (applied in order) ----
    $lines   = [System.Collections.Generic.List[string]]::new()
    $removed = [System.Collections.Generic.List[string]]::new()

    $inFrontmatter    = $false
    $frontmatterDone  = $false
    $frontmatterCount = 0
    $prevWasBlank     = $false
    $prevWasHeading   = $false

    foreach ($line in $originalLines) {

        # Track YAML frontmatter (never compress it)
        if (-not $frontmatterDone) {
            if ($line.Trim() -eq '---') {
                $frontmatterCount++
                if ($frontmatterCount -eq 2) { $frontmatterDone = $true }
                $lines.Add($line)
                $prevWasBlank   = $false
                $prevWasHeading = $false
                continue
            }
            if ($frontmatterCount -eq 1) {
                $lines.Add($line)
                continue
            }
        }

        # Rule 1: Collapse 3+ consecutive blank lines into 1
        if ($line.Trim() -eq '') {
            if ($prevWasBlank) {
                $removed.Add("(blank line)")
                continue
            }
            $prevWasBlank   = $true
            $prevWasHeading = $false
            $lines.Add($line)
            continue
        }
        $prevWasBlank = $false

        # Rule 2: Remove blank line immediately after a heading
        if ($prevWasHeading -and $line.Trim() -eq '') {
            $removed.Add("(blank after heading)")
            continue
        }
        $prevWasHeading = $line -match '^#{1,6}\s'

        # Rule 3: Remove lines that are pure filler phrases (verbose explanations)
        $fillerPatterns = @(
            '^\s*This (file|document|section) (describes|explains|defines|contains|provides|outlines)',
            '^\s*The purpose of this (file|document|section) is to',
            '^\s*Please (note|be aware|remember) that',
            '^\s*It (is|should be) (important|worth|noted) (to note|that)',
            '^\s*As (mentioned|described|noted|discussed) (above|below|earlier|previously)',
            '^\s*In (summary|conclusion|short|brief)[,:]?\s*$',
            '^\s*See (also|above|below)[.:]?\s*$',
            '^\s*\(end of (file|section|document)\)',
            '^\s*---+\s*$'   # horizontal rules outside frontmatter
        )
        $isFiller = $false
        foreach ($pat in $fillerPatterns) {
            if ($line -match $pat) { $isFiller = $true; break }
        }
        if ($isFiller) {
            $removed.Add($line.TrimEnd())
            continue
        }

        # Rule 4: Deduplicate identical non-blank lines (keep first occurrence)
        # Only applies outside code blocks to avoid mangling code
        $trimmed = $line.TrimEnd()
        $lines.Add($trimmed)
    }

    # Remove trailing blank lines
    while ($lines.Count -gt 0 -and $lines[$lines.Count - 1].Trim() -eq '') {
        $lines.RemoveAt($lines.Count - 1)
    }

    $compressed      = $lines -join "`n"
    $compressedLines = $lines.Count
    $compressedTokens = [math]::Round($compressed.Length / 4)
    $savedTokens     = $originalTokens - $compressedTokens
    $savedPct        = if ($originalTokens -gt 0) { [math]::Round(($savedTokens / $originalTokens) * 100) } else { 0 }

    Write-Host "  Compressed: $compressedLines lines, ~$compressedTokens tokens" -ForegroundColor Green
    Write-Host "  Saved:      ~$savedTokens tokens ($savedPct%)" -ForegroundColor Cyan
    Write-Host "  Removed:    $($removed.Count) lines" -ForegroundColor DarkGray
    Write-Host ""

    if ($removed.Count -gt 0) {
        Write-Host "Removed lines (sample, max 10):" -ForegroundColor DarkGray
        $removed | Select-Object -First 10 | ForEach-Object {
            Write-Host "  - $_" -ForegroundColor DarkGray
        }
        if ($removed.Count -gt 10) {
            Write-Host "  ... and $($removed.Count - 10) more" -ForegroundColor DarkGray
        }
        Write-Host ""
    }

    if ($Preview) {
        Write-Host "[Preview mode] No changes written." -ForegroundColor Yellow
        Write-Host "Remove -Preview to apply compression." -ForegroundColor Gray
        return
    }

    if ($savedTokens -le 0) {
        Write-Host "Nothing to compress - file is already compact." -ForegroundColor Green
        return
    }

    # Write backup
    $backupPath = "$fullPath.bak"
    Copy-Item -LiteralPath $fullPath -Destination $backupPath -Force
    Write-Host "Backup written: $(Split-Path $backupPath -Leaf)" -ForegroundColor DarkGray

    # Write compressed file
    [System.IO.File]::WriteAllText($fullPath, $compressed, [System.Text.Encoding]::UTF8)
    Write-Host "Compressed file written: $targetPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Review the compressed file to verify rules/examples are intact." -ForegroundColor Gray
    Write-Host "  2. Delete the .bak backup once satisfied." -ForegroundColor Gray
    Write-Host "  3. Run 'info -Path $targetPath' to confirm new token count." -ForegroundColor Gray
}

# ------------------------------------------------------------------
# COMMAND: HARVEST
# ------------------------------------------------------------------
#
# Harvest moves useful knowledge discovered during development into
# the correct permanent context file.
#
# Source can be:
#   - a path to an existing file (notes, temp doc, etc.)
#   - a raw string of text/notes to classify and store
#
# Target is the context category: architecture, patterns, packages,
#   examples, frontend, techstack. If omitted, auto-detected.
#
# Usage:
#   harvest -Source "notes.md" -Target patterns
#   harvest -Source "Always use sealed classes for handlers." -Target patterns
#   harvest -Source "notes.md"   (auto-detect target)

function Invoke-Harvest {
    if (-not $Source) {
        Write-Host 'Usage: harvest -Source <file-or-text> [-Target <category>]' -ForegroundColor Red
        Write-Host ""
        Write-Host "Categories: architecture, patterns, packages, examples, frontend, techstack" -ForegroundColor Gray
        Write-Host ""
        Write-Host "Examples:" -ForegroundColor Gray
        Write-Host "  harvest -Source `"notes.md`" -Target patterns" -ForegroundColor Yellow
        Write-Host "  harvest -Source `"Always use sealed classes for handlers.`" -Target patterns" -ForegroundColor Yellow
        Write-Host "  harvest -Source `"notes.md`"   (auto-detect category)" -ForegroundColor Yellow
        return
    }

    Write-Heading "Harvest"

    # --- Resolve source content ---
    $sourceContent = ""
    $sourceName    = ""
    $sourceIsFile  = $false

    $sourceFull = Join-Path $ProjectRoot $Source
    if (Test-Path $sourceFull -PathType Leaf) {
        $sourceContent = Get-Content $sourceFull -Raw -Encoding UTF8
        $sourceName    = Split-Path $sourceFull -Leaf
        $sourceIsFile  = $true
        $srcTokens = [math]::Round($sourceContent.Length / 4)
        Write-Host "  Source file: $Source (~$srcTokens tokens)" -ForegroundColor Gray
    }
    elseif (Test-Path (Join-Path $ProjectRoot (".opencode/" + $Source)) -PathType Leaf) {
        $sourceFull    = Join-Path $ProjectRoot (".opencode/" + $Source)
        $sourceContent = Get-Content $sourceFull -Raw -Encoding UTF8
        $sourceName    = Split-Path $sourceFull -Leaf
        $sourceIsFile  = $true
        $srcTokens = [math]::Round($sourceContent.Length / 4)
        Write-Host "  Source file: .opencode/$Source (~$srcTokens tokens)" -ForegroundColor Gray
    }
    else {
        # Treat Source as raw text
        $sourceContent = $Source
        $sourceName    = "inline-notes"
        $srcLen = $sourceContent.Length
        Write-Host "  Source: inline text - $srcLen chars" -ForegroundColor Gray
    }

    if (-not $sourceContent.Trim()) {
        Write-Host "Source is empty. Nothing to harvest." -ForegroundColor Yellow
        return
    }

    # --- Auto-detect target category if not specified ---
    $resolvedTarget = $Target
    if (-not $resolvedTarget) {
        $contentLower = $sourceContent.ToLower()
        $scores = @{
            architecture = 0
            patterns     = 0
            packages     = 0
            examples     = 0
            frontend     = 0
            techstack    = 0
        }

        # Simple keyword scoring for auto-detection
        $categoryKeywords = @{
            architecture = @("architecture","layer","dependency","clean architecture","cqrs","domain","application","infrastructure","api layer","boundary")
            patterns     = @("pattern","template","handler","command","query","repository","validator","convention","naming","anti-pattern","rule")
            packages     = @("nuget","package","install","version","namespace","using","import","library","dependency injection","di")
            examples     = @("example","sample","snippet","demo","usage","how to","e.g.","for instance")
            frontend     = @("vue","component","pinia","store","axios","bulma","router","composable","v-model","v-for")
            techstack    = @("tech stack","technology","framework",".net","c#","postgresql","dapper","mediatr","asp.net","overview")
        }

        foreach ($cat in $categoryKeywords.Keys) {
            foreach ($kw in $categoryKeywords[$cat]) {
                if ($contentLower -match [regex]::Escape($kw)) {
                    $scores[$cat] += 1
                }
            }
        }

        $best = $scores.GetEnumerator() | Sort-Object Value -Descending | Select-Object -First 1
        if ($best.Value -gt 0) {
            $resolvedTarget = $best.Key
            Write-Host "  Auto-detected category: $resolvedTarget (score: $($best.Value))" -ForegroundColor Cyan
        }
        else {
            Write-Host "  Could not auto-detect category. Please specify -Target." -ForegroundColor Yellow
            Write-Host "  Available: architecture, patterns, packages, examples, frontend, techstack" -ForegroundColor Gray
            return
        }
    }

    # --- Validate target ---
    if (-not $HarvestTargets.ContainsKey($resolvedTarget)) {
        Write-Host "Unknown target category: '$resolvedTarget'" -ForegroundColor Red
        Write-Host "Available: $($HarvestTargets.Keys -join ', ')" -ForegroundColor Gray
        return
    }

    $targetDir = Join-Path $ProjectRoot $HarvestTargets[$resolvedTarget]
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        Write-Host "  Created directory: $($HarvestTargets[$resolvedTarget])" -ForegroundColor DarkGray
    }

    # --- Derive output filename ---
    $baseName = $sourceName -replace '\.[^.]+$', ''   # strip extension
    $baseName = $baseName -replace '[^a-zA-Z0-9\-_]', '-' -replace '-{2,}', '-'
    $baseName = $baseName.ToLower().Trim('-')
    if ($baseName.Length -lt 3) { $baseName = "harvested-$(Get-Date -Format 'yyyyMMdd-HHmmss')" }

    $outputFile = Join-Path $targetDir "$baseName.md"

    # Avoid overwriting existing files - append timestamp suffix
    if (Test-Path $outputFile) {
        $ts = Get-Date -Format 'yyyyMMdd-HHmmss'
        $outputFile = Join-Path $targetDir "$baseName-$ts.md"
        Write-Host "  File already exists - using: $baseName-$ts.md" -ForegroundColor DarkYellow
    }

    # --- Build output with YAML frontmatter ---
    $domainMap = @{
        architecture = "backend"
        patterns     = "backend"
        packages     = "backend"
        examples     = "fullstack"
        frontend     = "frontend"
        techstack    = "project"
    }
    $domain = $domainMap[$resolvedTarget]

    $frontmatter = "---`r`ndomain: $domain`r`ncapabilities:`r`n  - $resolvedTarget`r`nkeywords:`r`n  - harvested`r`npriority: medium`r`ncost: low`r`n---`r`n`r`n"

    $outputContent = $frontmatter + $sourceContent.TrimStart()

    if ($Preview) {
        Write-Host ""
        Write-Host "Preview - would write to:" -ForegroundColor Cyan
        Write-Host "  $($HarvestTargets[$resolvedTarget])/$baseName.md" -ForegroundColor White
        Write-Host ""
        Write-Host "Content preview (first 20 lines):" -ForegroundColor DarkGray
        ($outputContent -split "`r`n|`n") | Select-Object -First 20 | ForEach-Object {
            Write-Host "  $_" -ForegroundColor DarkGray
        }
        Write-Host ""
        Write-Host "[Preview mode] No files written. Remove -Preview to apply." -ForegroundColor Yellow
        return
    }

    # --- Write output ---
    [System.IO.File]::WriteAllText($outputFile, $outputContent, [System.Text.Encoding]::UTF8)
    $relOutput = (Get-RelativePath $outputFile)

    Write-Host ""
    Write-Host "Harvested to: $relOutput" -ForegroundColor Green
    Write-Host "  Category:  $resolvedTarget" -ForegroundColor Gray
    Write-Host "  Domain:    $domain" -ForegroundColor Gray
    $outTokens = [math]::Round($outputContent.Length / 4)
    Write-Host "  Tokens:    ~$outTokens" -ForegroundColor Gray
    Write-Host ""

    if ($sourceIsFile) {
        Write-Host "Source file preserved at: $Source" -ForegroundColor DarkGray
        Write-Host "Delete it manually once you have verified the harvest." -ForegroundColor DarkGray
        Write-Host ""
    }

    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Open $relOutput and refine the YAML keywords/capabilities." -ForegroundColor Gray
    Write-Host "  2. Run 'discover -Task `"<related task>`"' to verify it surfaces correctly." -ForegroundColor Gray
    Write-Host "  3. Run 'compress -Path `"$relOutput`"' if the content is verbose." -ForegroundColor Gray
}

# ------------------------------------------------------------------
# COMMAND: ORGANIZE
# ------------------------------------------------------------------
#
# Organize inspects context files and reports files that are too large,
# mix multiple topics, or belong in a different category.
# It does NOT move or split files automatically - it produces a report
# with actionable recommendations.

function Invoke-Organize {
    Write-Heading "Organize - Context Health Report"
    Write-Host "Scanning all context files for size, focus and placement issues..." -ForegroundColor Gray
    Write-Host ""

    $allFiles = Get-AllContextFiles
    if ($allFiles.Count -eq 0) {
        Write-Host "No context files found." -ForegroundColor Yellow
        return
    }

    $issues = @()

    foreach ($file in $allFiles) {
        $meta = Get-FileMetadata $file
        $yaml = Get-YamlMetadata $file
        $rel  = $meta.RelPath

        # Issue 1: File is too large (> 300 lines or > 3000 tokens)
        if ($meta.Lines -gt 300 -or $meta.EstTokens -gt 3000) {
            $issues += [PSCustomObject]@{
                File       = $rel
                Severity   = "HIGH"
                Issue      = "Too large"
                Detail     = "$($meta.Lines) lines, ~$($meta.EstTokens) tokens - consider splitting into focused files"
                Action     = "Split into smaller files by topic"
            }
        }

        # Issue 2: No YAML frontmatter
        if ($yaml.Count -eq 0 -or (-not $yaml.Domain -and -not $yaml.Keywords)) {
            $issues += [PSCustomObject]@{
                File       = $rel
                Severity   = "MEDIUM"
                Issue      = "Missing metadata"
                Detail     = "No YAML frontmatter - file will not surface in discovery"
                Action     = "Add domain, capabilities, keywords, priority, cost"
            }
        }

        # Issue 3: High cost flag but no justification (large file without high priority)
        if ($yaml.Cost -eq 'high' -and $yaml.Priority -ne 'high' -and $meta.EstTokens -lt 500) {
            $issues += [PSCustomObject]@{
                File       = $rel
                Severity   = "LOW"
                Issue      = "Cost/priority mismatch"
                Detail     = "Marked cost:high but file is small (~$($meta.EstTokens) tokens)"
                Action     = "Review cost metadata - may be incorrectly tagged"
            }
        }

        # Issue 4: File in wrong category (heuristic: frontend keywords in backend path)
        if ($rel -like "*.opencode/context/architecture/*" -or $rel -like "*.opencode/context/patterns/*") {
            $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
            if ($content -and ($content -match '\bvue\b|\bpinia\b|\bcomponent\b|\baxios\b' -and $content -notmatch '\bbackend\b|\bhandler\b|\bcommand\b')) {
                $issues += [PSCustomObject]@{
                    File       = $rel
                    Severity   = "MEDIUM"
                    Issue      = "Possible wrong category"
                    Detail     = "File contains frontend keywords but lives in backend context"
                    Action     = "Move to .opencode/context/frontend/"
                }
            }
        }

        # Issue 5: Empty or near-empty file
        if ($meta.Lines -lt 5 -and $meta.Lines -gt 0) {
            $issues += [PSCustomObject]@{
                File       = $rel
                Severity   = "LOW"
                Issue      = "Near-empty file"
                Detail     = "Only $($meta.Lines) lines - may be a stub or leftover"
                Action     = "Complete, merge into another file, or delete"
            }
        }
    }

    if ($issues.Count -eq 0) {
        Write-Host "No issues found. Context files look healthy." -ForegroundColor Green
        Write-Host "Total files scanned: $($allFiles.Count)" -ForegroundColor Gray
        return
    }

    # Group by severity
    $high   = @($issues | Where-Object { $_.Severity -eq "HIGH" })
    $medium = @($issues | Where-Object { $_.Severity -eq "MEDIUM" })
    $low    = @($issues | Where-Object { $_.Severity -eq "LOW" })

    Write-Host "Found $($issues.Count) issue(s) across $($allFiles.Count) files:" -ForegroundColor Yellow
    Write-Host ""

    if ($high.Count -gt 0) {
        Write-Host "HIGH ($($high.Count))" -ForegroundColor Red
        Write-Host ("-" * 60) -ForegroundColor DarkGray
        foreach ($i in $high) {
            Write-Host "  $($i.File)" -ForegroundColor White
            Write-Host "  Issue:  $($i.Issue) - $($i.Detail)" -ForegroundColor Gray
            Write-Host "  Action: $($i.Action)" -ForegroundColor Cyan
            Write-Host ""
        }
    }

    if ($medium.Count -gt 0) {
        Write-Host "MEDIUM ($($medium.Count))" -ForegroundColor Yellow
        Write-Host ("-" * 60) -ForegroundColor DarkGray
        foreach ($i in $medium) {
            Write-Host "  $($i.File)" -ForegroundColor White
            Write-Host "  Issue:  $($i.Issue) - $($i.Detail)" -ForegroundColor Gray
            Write-Host "  Action: $($i.Action)" -ForegroundColor Cyan
            Write-Host ""
        }
    }

    if ($low.Count -gt 0) {
        Write-Host "LOW ($($low.Count))" -ForegroundColor DarkGray
        Write-Host ("-" * 60) -ForegroundColor DarkGray
        foreach ($i in $low) {
            Write-Host "  $($i.File)" -ForegroundColor White
            Write-Host "  Issue:  $($i.Issue) - $($i.Detail)" -ForegroundColor Gray
            Write-Host "  Action: $($i.Action)" -ForegroundColor DarkGray
            Write-Host ""
        }
    }

    Write-Host "Recommended actions:" -ForegroundColor Cyan
    Write-Host '  Split large files:   harvest -Source <file> -Target <category>' -ForegroundColor Gray
    Write-Host "  Compress verbose:    compress -Path <file>" -ForegroundColor Gray
    Write-Host "  Add metadata:        edit the file and add YAML frontmatter" -ForegroundColor Gray

    if ($Json) {
        return $issues | ConvertTo-Json
    }
}

# ------------------------------------------------------------------
# COMMAND: PROMOTE
# ------------------------------------------------------------------
#
# Promote moves a reviewed harvest file from .harvest/<category>/
# into the curated context folder context/<category>/.
#
# Usage:
#   promote -Path ".opencode/context/.harvest/patterns/new-pattern.md"
#   promote -Path ".opencode/context/.harvest/patterns/new-pattern.md" -Preview

function Invoke-Promote {
    if ($Path.Count -eq 0) {
        Write-Host 'Usage: promote -Path <harvest-file-path>' -ForegroundColor Red
        Write-Host "Example: promote -Path `".opencode/context/.harvest/patterns/new-pattern.md`"" -ForegroundColor Gray
        return
    }

    $sourcePath = $Path[0]
    $fullSource = Join-Path $ProjectRoot $sourcePath
    if (-not (Test-Path $fullSource)) {
        $altSource = Join-Path $ProjectRoot ".opencode/$sourcePath"
        if (Test-Path $altSource) { $fullSource = $altSource; $sourcePath = ".opencode/$sourcePath" }
        else {
            Write-Host "File not found: $sourcePath" -ForegroundColor Red
            return
        }
    }

    $relSource = Get-RelativePath $fullSource

    # Validate file is in .harvest/
    if ($relSource -notlike "*/.harvest/*") {
        Write-Host "File is not in a .harvest/ directory: $relSource" -ForegroundColor Red
        Write-Host "Only files in .opencode/context/.harvest/<category>/ can be promoted." -ForegroundColor Gray
        return
    }

    # Validate YAML frontmatter exists
    $yaml = Get-YamlMetadata $fullSource
    if ($yaml.Count -eq 0 -or (-not $yaml.Domain -and -not $yaml.Keywords)) {
        Write-Host "File has no YAML frontmatter. Add metadata before promoting." -ForegroundColor Yellow
        Write-Host "Required: domain, capabilities, keywords, priority, cost" -ForegroundColor Gray
        return
    }

    # Determine target path: replace .harvest/<category>/ with <category>/
    $targetRel = $relSource -replace '/\.harvest/', '/'
    $fullTarget = Join-Path $ProjectRoot $targetRel

    $targetDir = Split-Path $fullTarget -Parent
    if (-not (Test-Path $targetDir)) {
        if (-not $Preview) {
            New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        }
    }

    Write-Heading "Promote"
    Write-Host "  Source:  $relSource" -ForegroundColor Gray
    Write-Host "  Target:  $targetRel" -ForegroundColor Cyan
    Write-Host "  Domain:  $($yaml.Domain)" -ForegroundColor DarkGray
    if ($yaml.Keywords) {
        Write-Host "  Keywords: $($yaml.Keywords -join ', ')" -ForegroundColor DarkGray
    }

    if (Test-Path $fullTarget) {
        Write-Host ""
        Write-Host "  [!] Target file already exists. Promotion would overwrite it." -ForegroundColor Yellow
        Write-Host "  Rename the harvest file or delete the existing target first." -ForegroundColor Gray
        return
    }

    if ($Preview) {
        Write-Host ""
        Write-Host "[Preview mode] No files moved. Remove -Preview to apply." -ForegroundColor Yellow
        return
    }

    # Move file
    Move-Item -LiteralPath $fullSource -Destination $fullTarget -Force
    Write-Host ""
    Write-Host "Promoted: $targetRel" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Run 'discover -Task `"<related task>`"' to verify it surfaces correctly." -ForegroundColor Gray
    Write-Host "  2. Run 'info -Path `"$targetRel`"' to confirm metadata." -ForegroundColor Gray
}

# ------------------------------------------------------------------
# COMMAND: STALE
# ------------------------------------------------------------------
#
# Stale reports context files that may be outdated based on the
# optional `updated` YAML metadata field or file modification date.
#
# Usage:
#   stale                          # files not updated in 90+ days
#   stale -MaxFiles 10             # limit output
#   stale -MaxTokens 0             # use 0 to mean "show all, no token filter"

function Invoke-Stale {
    $thresholdDays = 90
    Write-Heading "Stale Context Files (>$thresholdDays days)"
    Write-Host ""

    $allFiles = Get-AllContextFiles
    $now = Get-Date
    $staleFiles = @()
    $missingDate = @()

    foreach ($file in $allFiles) {
        $yaml = Get-YamlMetadata $file
        $meta = Get-FileMetadata $file
        $rel  = $meta.RelPath

        # Check YAML updated field first
        $updatedDate = $null
        $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
        if ($content -and $content -match '(?m)^updated:\s*(\d{4}-\d{2}-\d{2})') {
            $updatedDate = [datetime]::ParseExact($matches[1], 'yyyy-MM-dd', $null)
        }

        if ($updatedDate) {
            $age = ($now - $updatedDate).Days
            if ($age -gt $thresholdDays) {
                $staleFiles += [PSCustomObject]@{
                    File = $rel; Age = $age; Source = "yaml"; Tokens = $meta.EstTokens
                }
            }
        } else {
            # Fall back to file modification date
            $lastWrite = (Get-Item $file).LastWriteTime
            $age = ($now - $lastWrite).Days
            if ($age -gt $thresholdDays) {
                $staleFiles += [PSCustomObject]@{
                    File = $rel; Age = $age; Source = "filesystem"; Tokens = $meta.EstTokens
                }
            }
            $missingDate += $rel
        }
    }

    if ($staleFiles.Count -eq 0) {
        Write-Host "No stale files found. All context files are within $thresholdDays days." -ForegroundColor Green
        if ($missingDate.Count -gt 0) {
            Write-Host ""
            Write-Host "$($missingDate.Count) files have no 'updated' YAML field (using filesystem date)." -ForegroundColor DarkGray
        }
        return
    }

    $sorted = $staleFiles | Sort-Object Age -Descending
    if ($MaxFiles -gt 0) { $sorted = $sorted | Select-Object -First $MaxFiles }

    Write-Host ("{0,5} {1,-55} {2,6} {3}" -f "Days", "File", "Tokens", "Source") -ForegroundColor Cyan
    Write-Host ("-" * 80) -ForegroundColor DarkGray

    foreach ($f in $sorted) {
        $color = if ($f.Age -gt 180) { "Red" } elseif ($f.Age -gt 120) { "Yellow" } else { "White" }
        Write-Host ("{0,5} {1,-55} {2,6} {3}" -f $f.Age, $f.File, "~$($f.Tokens)", $f.Source) -ForegroundColor $color
    }

    Write-Host ""
    Write-Host "Found $($staleFiles.Count) stale files (>$thresholdDays days)" -ForegroundColor Yellow
    if ($missingDate.Count -gt 0) {
        Write-Host "$($missingDate.Count) files have no 'updated' YAML field — add 'updated: YYYY-MM-DD' to metadata" -ForegroundColor DarkGray
    }
    Write-Host ""
    Write-Host "To refresh a file: edit it, update content, set 'updated: $(Get-Date -Format 'yyyy-MM-dd')' in YAML" -ForegroundColor Gray
}

# ------------------------------------------------------------------
# MAIN DISPATCH
# ------------------------------------------------------------------

if ($Help) {
    Get-Help $MyInvocation.MyCommand.Path -Detailed
    exit
}

switch ($Command) {
    "discover" { Invoke-Discover }
    "load"     { Invoke-Load }
    "tree"     { Invoke-Tree }
    "groups"   { Invoke-Groups }
    "info"     { Invoke-Info }
    "search"   { Invoke-Search }
    "keywords" { Invoke-Keywords }
    "harvest"  { Invoke-Harvest }
    "compress" { Invoke-Compress }
    "organize" { Invoke-Organize }
    "promote"  { Invoke-Promote }
    "stale"    { Invoke-Stale }
}
