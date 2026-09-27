---
name: build-verify
description: Build the .NET backend or Vue 3 frontend and verify compilation succeeds. Use after code changes, before running tests.
---

# Build Verify

Load this skill in the CHECK step before running tests.

## Backend (.NET)

```powershell
dotnet build
```

- Exit code 0 → **PASS**
- Errors → report them, return **FAIL**

## Frontend (Vue 3)

```powershell
npm run build
```

- Exit code 0 → **PASS**
- Errors → report them, return **FAIL**

## Outcome

- **PASS:** proceed to next gate (TEST)
- **FAIL:** diagnose errors, fix, rebuild. Do not proceed until PASS.
