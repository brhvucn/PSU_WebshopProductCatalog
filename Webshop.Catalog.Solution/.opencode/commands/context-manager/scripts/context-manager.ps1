<#
.SYNOPSIS
    Context Manager — forwarder to the main script at .opencode/scripts/context-manager.ps1
#>
& (Join-Path $PSScriptRoot "..\..\..\scripts\context-manager.ps1") @args
