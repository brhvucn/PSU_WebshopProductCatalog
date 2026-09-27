---
name: create-improvement
description: Create an improvement artifact when agent system friction is discovered.
---

# Create Improvement Skill

## File

```
.artifacts/improvements/YYYY-MM-DD-short-title.md
```

Use `.opencode/templates/improvement-template.md` — all fields required.

## After Creating

- If BLOCKER/HIGH: append to SESSION.md Blockers
- If fix is one-line: apply immediately, set `fixed`
- Otherwise: leave `open`

## Rules

- Improvements are about the agent system (`.opencode/`), NOT the software project
- Fix Candidate must name a specific `.opencode/` file to change
- Software issues go to `.artifacts/risks/` or `.artifacts/decisions/`
