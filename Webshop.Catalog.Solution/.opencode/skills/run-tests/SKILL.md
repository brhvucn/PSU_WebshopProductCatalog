---
name: run-tests
description: Run the test suite — xUnit for .NET backend, Vitest for Vue 3 frontend. Use after build passes, before commit.
---

# Run Tests

Load this skill in the CHECK step after BUILD passes.

## Backend (.NET)

```powershell
dotnet test
```

## Frontend (Vue 3)

```powershell
npx vitest run
```

## Test Report

| Item | Value |
|---|---|
| Tests run | N |
| Passed | N |
| Failed | N |
| Verdict | PASS / FAIL |

- All tests pass → **PASS**
- Any test fails → **FAIL** — report which tests failed and their error messages

## Outcome

- **PASS:** proceed to next gate (REVIEW or SECURITY, or COMMIT)
- **FAIL:** diagnose, fix, rebuild, retest. Do not proceed until PASS.
