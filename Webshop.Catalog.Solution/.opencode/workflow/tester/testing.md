# Testing Workflow

## Purpose

The testing workflow validates that implemented milestone functionality behaves
correctly and that the milestone is safe to continue or release.

Testing is **optional** — invoke with `@tester` when dedicated test validation is desired.
For most milestones, tests are written and run inline by `ucn-developer`.

The purpose is to:

- execute existing tests and report results
- identify missing test coverage for the active milestone
- validate implementation behavior against requirements
- identify regressions introduced by the current milestone
- produce a clear verdict on release readiness

The workflow does not require perfect test coverage.
It requires sufficient confidence that the milestone works as intended.

---

## Workflow Owner

Primary owner:

```txt
tester
```

Supporting participants:

```txt
developer
```

---

## Allowed Instructions

| Instruction | Purpose |
|---|---|
| `/test` | Execute tests and produce test report |
| `/create-risk` | Register testing risks and gaps |

The workflow must not modify implementation. Findings go to the developer.

---

## Workflow Sequence

```txt
load active milestone context from SESSION.md
    ->
dotnet build  (verify build succeeds first)
    ->
dotnet test   (run all tests)
    ->
analyse: passed / failed / skipped / coverage gaps
    ->
produce test report in .artifacts/testing/
    ->
verdict: PASS / FAIL / PARTIAL
    ->
FAIL  -> return to developer with findings
PASS / PARTIAL -> notify developer, proceed to release if desired
```

---

## Test Report

Produce `.artifacts/testing/YYYY-MM-DD-[milestone]-test-report.md`:

| Field | Content |
|---|---|
| Milestone | Active milestone name |
| Date | YYYY-MM-DD |
| Tests run | Total count |
| Passed | Count |
| Failed | Count + names |
| Skipped | Count |
| Coverage gaps | Features with no tests |
| Verdict | PASS / FAIL / PARTIAL |
| Next step | What should happen next |

---

## Verdict Definitions

| Verdict | Meaning | Next step |
|---|---|---|
| `PASS` | All tests pass, no critical gaps | Proceed |
| `PARTIAL` | Tests pass but coverage gaps exist | Proceed with gaps noted |
| `FAIL` | One or more tests fail | Return to developer — do not release |

---

## Operating Principle

```txt
Validate continuously. Make gaps visible. Never hide failing tests.
```
