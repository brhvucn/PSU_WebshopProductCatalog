---
purpose: Validate implementation quality and architecture alignment
owner: reviewer (invoked by developer or task loop)
supports: [developer, architect]
phase: build
artifacts:
  produce:
    - .artifacts/reviews/implementation-review-YYYY-MM-DD.md
    - .artifacts/risks/ (if gaps found)
  read:
    - .artifacts/implementation/
    - .artifacts/architecture/
    - .artifacts/planning/milestones/
    - src/
entryFrom: [implementation, task-loop (if [high-risk] tag)]
exitTo: [implementation (if pass), fix (if fail)]
commands: [/review, /create-risk]
verdict: [APPROVED, APPROVED WITH NOTES, CHANGES REQUIRED]
---

# Implementation Review Workflow

Validate implementation quality, architecture alignment, and readiness.

Triggered: Automatically for `[high-risk]` tasks, or ask user for milestone review.

---

## Review Checklist

**Architecture Alignment:**
- [ ] Follows 3-layer pattern (Api → Business → Data)
- [ ] No layer violations (Api → Data forbidden)
- [ ] Service boundaries clear
- [ ] Repository pattern followed

**Code Quality:**
- [ ] No code duplication (DRY)
- [ ] Single responsibility principle
- [ ] Clear naming conventions
- [ ] Appropriate error handling

**Testing:**
- [ ] Tests exist for implementation
- [ ] Tests pass (BUILD + TEST gates)
- [ ] Coverage adequate for task
- [ ] Edge cases covered

**Task Completion:**
- [ ] Task scope complete
- [ ] Acceptance criteria met
- [ ] No work-in-progress left
- [ ] Commit message clear

**Dependencies:**
- [ ] No unexpected dependencies added
- [ ] Dependency injection used correctly
- [ ] Database migrations exist (if schema changed)

**Security (if `[security]` tag):**
- [ ] @security gate passed
- [ ] No security vulnerabilities
- [ ] Auth/authz correctly implemented

---

## Verdict

| Verdict | Meaning | Next Step |
|---------|---------|-----------|
| **APPROVED** | Ready to commit | Proceed to COMMIT |
| **APPROVED WITH NOTES** | Commit, address notes later | Proceed to COMMIT, log notes |
| **CHANGES REQUIRED** | Fix before commit | Return to DO, fix issues |

---

## Common Issues

**CHANGES REQUIRED if:**
- Layer violations (Api → Data direct)
- No tests
- Tests failing
- Task scope incomplete
- Security issues (if `[security]` tag)
- Code duplicates existing patterns (should reuse)

**APPROVED WITH NOTES if:**
- Minor naming improvements
- Optional refactoring opportunities
- Non-critical optimizations

**APPROVED if:**
- All checklist items pass
- Ready to commit

---

## Review Artifact

**Template:** `.opencode/templates/review-template.md`

**File:** `.artifacts/reviews/implementation-review-YYYY-MM-DD.md`

---

## Workflow Boundaries

**Entry:**
- DO step complete
- Code + tests written
- Task tagged `[high-risk]` (automatic)
- Or user requested review (ask)

**Exit (APPROVED):**
- Proceed to COMMIT
- Continue task loop

**Exit (CHANGES REQUIRED):**
- Return to DO
- Fix issues
- Re-run CHECK gates

**Must not:**
- Approve incomplete work
- Skip validation
- Modify code directly (read-only)

---

## Operating Principle

```
Validate incrementally. Make findings explicit. Never hide quality assumptions.
```
