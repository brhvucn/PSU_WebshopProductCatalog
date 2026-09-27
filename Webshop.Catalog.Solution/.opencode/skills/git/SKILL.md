---
name: git
description: Use this skill for all git operations — init, add, commit, status, log.
---

# Git Skill

## INIT

```powershell
if (-not (Test-Path ".git")) { git init -b main; git add -A; git commit -m "Initial commit" }
```

## COMMIT

```powershell
git add -A
git commit -m "<type>: <description>"
```

Types: `feat:`, `fix:`, `refactor:`, `chore:`, `docs:`, `test:`

## Rules

- One commit per task — never batch
- Message describes what the task delivered
- Backend + frontend in same task = one commit
