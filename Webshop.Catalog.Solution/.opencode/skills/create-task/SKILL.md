---
name: create-task
description: Create a tracked task with tags for the active milestone.
---

# Create Task Skill

## Format

```
- [ ] Description [tag]
```

## Tags

| Tag | Effect |
|---|---|
| `[security]` | Triggers @security after DO |
| `[high risk]` | Triggers @reviewer after DO |
| `[test]` | Triggers @tester after DO |

Multiple: `- [ ] Payment flow [security] [high risk]`

## Location

- Task list: `.artifacts/planning/milestones/<name>.md`
- Individual task file (optional): `.artifacts/tasks/<name>.md`

## Rules

Tasks must be independently implementable, testable, and commitable.
Emergent tasks discovered mid-implementation: add `- [ ]` immediately.
