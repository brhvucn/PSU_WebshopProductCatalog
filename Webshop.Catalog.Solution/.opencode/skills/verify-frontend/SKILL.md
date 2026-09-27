---
name: verify-frontend
description: Use this skill to build and test Vue 3 frontend code before committing.
---

# Verify Frontend Skill

## BUILD + TEST

```powershell
npm run build
npx vitest run
```

## Rules

- Run both build and test before marking any frontend task complete
- Fix all build errors and test failures before proceeding to COMMIT
- 3 failures → mark task `[~]`, create risk artifact, escalate
