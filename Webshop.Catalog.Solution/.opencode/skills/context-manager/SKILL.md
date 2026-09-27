---
name: context-manager
description: Use this skill to discover, load, organize and maintain project context for .NET projects before making implementation or architecture changes. Also use when you need to understand the context hierarchy, find relevant files, or estimate token costs.
---

# Context Manager Skill

This skill manages project context for .NET projects using the context-manager script.
All agents MUST use this skill before any implementation or architecture work.

## Tool

The context manager script lives at:

```
.opencode/scripts/context-manager.ps1
```

Use it before:
- implementing a new feature
- changing architecture
- creating commands, queries, handlers or endpoints
- modifying persistence
- using shared NuGet packages
- changing project structure
- updating agent instructions
- **anytime you need to understand what context is available**

## Core principle

```
Do not load all context.
First discover, then load selectively.
```

The full project context is ~72K tokens. Never load it all at once. Use the tool to discover what is relevant, then load only what you need.

## Quick-start workflow

```
# Step 1: Discover what is relevant
.opencode\scripts\context-manager.ps1 discover -Task "your task description"

# Step 2: Load only what you need (by path or group)
.opencode\scripts\context-manager.ps1 load -Group cqrs
.opencode\scripts\context-manager.ps1 load -Path "context/architecture/cqrs.md"

# Step 3: Check token cost before proceeding
.opencode\scripts\context-manager.ps1 info -Path "context/patterns/command-template.md"
```

## Context folders

- `.opencode/context/architecture/` — 12 files, ~4.8K tokens
- `.opencode/context/patterns/` — 14 files, ~9.2K tokens (largest category)
- `.opencode/context/packages/` — 9 files, ~7.9K tokens
- `.opencode/context/examples/` — 10 files, ~4.7K tokens
- `.opencode/context/frontend/` — 10 files, ~3K tokens
- `.opencode/templates/` — 16 templates across 4 subdirectories
- `.opencode/workflow/` — 14 workflow files
- `.opencode/instructions/` — 22 instruction files
- `.artifacts/` — produced deliverables

## Available Commands

### discover

Search context files by keyword or task description. Returns ranked results.

```
.opencode\scripts\context-manager.ps1 discover -Task "Create ticket query handler"
.opencode\scripts\context-manager.ps1 discover -Keyword "cqrs,command,handler"
```

Files are ranked by relevance (filename match > path match > content match).

### load

Load specific context files or predefined groups. Always shows token estimates.

```
# By path (one or more files):
.opencode\scripts\context-manager.ps1 load -Path "context/architecture/cqrs.md"
.opencode\scripts\context-manager.ps1 load -Path "context/architecture/cqrs.md","context/patterns/command-handler-template.md"

# By group (predefined sets):
.opencode\scripts\context-manager.ps1 load -Group cqrs
.opencode\scripts\context-manager.ps1 load -Group database
.opencode\scripts\context-manager.ps1 load -Group api
.opencode\scripts\context-manager.ps1 load -Group frontend
.opencode\scripts\context-manager.ps1 load -Group patterns
.opencode\scripts\context-manager.ps1 load -Group packages

# Load templates ON DEMAND (only when creating new artifacts):
.opencode\scripts\context-manager.ps1 load -Group templates

# Load code examples ON DEMAND (only when implementing frontend):
.opencode\scripts\context-manager.ps1 load -Group examples

# Minimal startup context (techstack overview):
.opencode\scripts\context-manager.ps1 load -Group minimal
```

### tree

Visualize the deep hierarchy with file sizes.

```
.opencode\scripts\context-manager.ps1 tree
```

### groups

List all predefined context groups with file counts and token estimates.

```
.opencode\scripts\context-manager.ps1 groups
```

### info

Get metadata about a specific file (lines, tokens, depth, sections).

```
.opencode\scripts\context-manager.ps1 info -Path "context/patterns/repository-template.md"
```

### search

Full-text search across all context files.

```
.opencode\scripts\context-manager.ps1 search -Keyword "Result<T>,Failure"
```

### organize

If a context file is too large or mixes many topics, split it into smaller focused files.

### harvest

If useful information is discovered during development, move it from temporary notes into the correct permanent context file.

```
.opencode\scripts\context-manager.ps1 harvest -Source "<file-or-text>" -Target <category>
```

### compress

Shorten context files without removing rules, examples or decisions.

```
.opencode\scripts\context-manager.ps1 compress -Path "<context-file>" -Preview
.opencode\scripts\context-manager.ps1 compress -Path "<context-file>"
```

## On-Demand Loading Strategy

| Content Type | When To Load | Cost | Load Command |
|---|---|---|---|
| **Templates** | Only when creating new artifacts | ~500 tokens | `load -Group templates` |
| **Code Examples** | Only when implementing frontend features | ~4.7K tokens | `load -Group examples` |
| **Deep hierarchies** | Only when navigating/workflow design | ~500 tokens | `tree`; `load` specific files |
| **Frontend** | Only when doing frontend work | ~7K tokens | `load -Group frontend` |
| **Architecture** | Always at start of task, but only 2-3 files | ~2-4K tokens | `discover` then `load -Path` |
| **Patterns** | When implementing specific patterns | ~1-5K tokens | `load -Group cqrs/database/api` |
| **Packages** | When adding/using NuGet packages | ~500-2K tokens | `load -Group packages` |

## Output format

When using this skill, respond with:

```md
## Context loaded

Relevant files:
- `.opencode/context/architecture/cqrs.md` (~300 tokens)
- `.opencode/context/patterns/command-template.md` (~165 tokens)

Total: ~465 tokens

Next action:
```

---
