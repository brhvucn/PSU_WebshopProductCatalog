---
purpose: Validate implementation readiness — sufficient milestone planning exists
owner: reviewer (invoked by consultant)
supports: [consultant, architect, developer]
phase: understand
artifacts:
  produce:
    - .artifacts/reviews/planning-review-YYYY-MM-DD.md
    - .artifacts/risks/ (if gaps found)
  read:
    - .artifacts/planning/
    - .artifacts/architecture/
entryFrom: [planning]
exitTo: [implementation, planning (if changes required)]
commands: [/review, /create-risk, /create-decision]
verdict: [APPROVED, APPROVED WITH NOTES, CHANGES REQUIRED]
---

# Planning Review Workflow

Validate implementation readiness: milestones structured, tasks right-sized, dependencies clear, risks visible.

Goal: Ensure implementation can begin responsibly while supporting agile delivery and incremental refinement.

---

## Review Checklist

**Milestone Structure:**
- [ ] At least 1 milestone defined
- [ ] Maximum 5 milestones initially (defer long-term planning)
- [ ] Each milestone 5-15 tasks
- [ ] Each milestone independently testable
- [ ] First milestone is foundational (database, core services, basic API)

**Task Quality:**
- [ ] Tasks independently implementable
- [ ] Tasks independently testable
- [ ] Tasks produce one commit each
- [ ] Tasks right-sized (1 loop iteration)
- [ ] Backend/frontend tasks clearly separated

**Tagging:**
- [ ] `[security]` tag on auth/data/endpoint tasks
- [ ] `[high-risk]` tag on complex/cross-cutting tasks
- [ ] Tags trigger correct gates (security → @security, high-risk → @reviewer)

**Dependencies:**
- [ ] Dependencies between tasks identified
- [ ] Dependencies sequenced correctly
- [ ] External dependencies flagged as risks
- [ ] No circular dependencies

**Risks:**
- [ ] Planning risks identified
- [ ] Blockers visible
- [ ] Mitigation strategies exist

**Clarity:**
- [ ] Task descriptions clear (what to implement)
- [ ] Acceptance criteria clear (when task is done)
- [ ] No ambiguity in scope

**Realism:**
- [ ] Milestones achievable in 1-2 weeks
- [ ] No unbounded scope
- [ ] Defer future work (not all features upfront)

---

## Verdict

Every review must conclude with:

| Verdict | Meaning | Next Step |
|---------|---------|-----------|
| **APPROVED** | Ready to implement | Transition to implementation |
| **APPROVED WITH NOTES** | Proceed, address notes later | Transition to implementation, log notes |
| **CHANGES REQUIRED** | Fix before implementing | Return to planning, address gaps |

---

## Common Issues

**CHANGES REQUIRED if:**
- No milestones defined
- Milestones > 15 tasks (split them)
- Tasks too large (not independently implementable)
- Missing tags on security/high-risk tasks
- Unclear dependencies
- First milestone not foundational
- Unbounded scope (trying to plan everything upfront)

**APPROVED WITH NOTES if:**
- Minor task description clarity issues
- Optional optimization opportunities
- Suggested refactorings for later milestones

**APPROVED if:**
- All checklist items pass
- Milestones clear
- Implementation can begin responsibly

---

## Review Artifact

**Template:** `.opencode/templates/review-template.md`

**Required sections:**
- Verdict (APPROVED / APPROVED WITH NOTES / CHANGES REQUIRED)
- Checklist results
- Findings (if any)
- Recommendations (if any)
- Risks identified (if any)

**File:** `.artifacts/reviews/planning-review-YYYY-MM-DD.md`

---

## Workflow Boundaries

**Entry:**
- Planning complete
- Milestones defined
- Tasks structured

**Exit (APPROVED):**
- Transition to implementation
- Tell user to switch to `ucn-developer` (Tab)

**Exit (CHANGES REQUIRED):**
- Return to planning
- Address gaps
- Re-submit for review

**Must not:**
- Approve incomplete planning
- Perform implementation
- Skip validation

---

## Operating Principle

```
Validate incrementally. Ensure implementation readiness. Make gaps visible.
Support agile delivery. Don't require perfection.
```
