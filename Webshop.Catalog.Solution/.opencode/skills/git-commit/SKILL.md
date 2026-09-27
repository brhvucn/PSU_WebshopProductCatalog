---
name: git-commit
description: Stage all changes and create one conventional commit per completed task. Use after all verification gates pass.
---

# Git Commit

Load this skill after all CHECK gates (BUILD, TEST, REVIEW, SECURITY) pass.

## Command

```powershell
git add -A; git commit -m "<type>: <task description>"
```

## Commit Rules

- **One commit per task** — never batch multiple tasks
- Conventional commit types:
  - `feat:` — new feature
  - `fix:` — bug fix
  - `refactor:` — code restructuring
  - `chore:` — maintenance, tooling
  - `docs:` — documentation
  - `test:` — testing
- Message must describe what the task delivered

## Outcome

- Commit succeeds → proceed to TRACK step
- Commit fails → diagnose (staged changes? git config?), fix, retry
