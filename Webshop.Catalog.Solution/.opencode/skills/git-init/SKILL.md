---
name: git-init
description: Initialize a git repository in the project root if none exists. Use at session startup before any task work.
---

# Git Init

Load this skill at session startup before any task work.

## Check and Initialize

```powershell
if (-not (Test-Path -LiteralPath ".git")) {
    git init -b main
    git add -A
    git commit -m "Initial commit"
}
```

## Outcome

- Repository exists after execution → proceed with task loop
- If init fails → report error, do not start task work
