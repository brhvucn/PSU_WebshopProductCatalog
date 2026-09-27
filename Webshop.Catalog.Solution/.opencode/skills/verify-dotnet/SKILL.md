---
name: verify-dotnet
description: Use this skill to build and test .NET backend code before committing.
---

# Verify .NET Skill

## BUILD + TEST

```powershell
dotnet build
dotnet test
```

## Rules

- Run both build and test before marking any task complete
- Fix all build errors and test failures before proceeding to COMMIT
- 3 failures → mark task `[~]`, create risk artifact, escalate
