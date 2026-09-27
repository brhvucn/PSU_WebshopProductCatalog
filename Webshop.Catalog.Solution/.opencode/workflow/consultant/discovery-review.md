---
purpose: Validate discovery quality and architecture readiness
owner: reviewer (invoked by consultant)
supports: [consultant, architect]
phase: understand
artifacts:
  produce:
    - .artifacts/reviews/discovery-review-YYYY-MM-DD.md
    - .artifacts/risks/ (if gaps found)
  read:
    - .artifacts/discovery/
    - .artifacts/intake/
    - .artifacts/clarification/
entryFrom: [discovery]
exitTo: [architecture, discovery (if changes required)]
commands: [/review, /create-risk]
verdict: [APPROVED, APPROVED WITH NOTES, CHANGES REQUIRED]
---

# Discovery Review Workflow

Validate discovery quality: business value clear, stakeholders identified, constraints understood.

---

## Review Checklist

**Business Value:**
- [ ] Problem statement clear
- [ ] Value proposition identified
- [ ] Success criteria defined
- [ ] User needs understood

**Stakeholders:**
- [ ] Key stakeholders identified
- [ ] Roles and responsibilities clear
- [ ] Decision makers known
- [ ] Communication channels established

**Constraints:**
- [ ] Technical constraints identified
- [ ] Budget/timeline constraints clear
- [ ] Organizational constraints understood
- [ ] Legal/compliance requirements known

**Scope:**
- [ ] In-scope features identified
- [ ] Out-of-scope features clear
- [ ] MVP defined
- [ ] Future phases identified

**Risks:**
- [ ] Business risks identified
- [ ] Technical risks visible
- [ ] Dependency risks clear
- [ ] Mitigation strategies exist

**Clarity:**
- [ ] No critical ambiguity
- [ ] Assumptions documented
- [ ] Unknowns flagged

---

## Verdict

| Verdict | Meaning | Next Step |
|---------|---------|-----------|
| **APPROVED** | Ready for architecture | Transition to architecture |
| **APPROVED WITH NOTES** | Proceed, clarify later | Transition to architecture, log notes |
| **CHANGES REQUIRED** | Clarify before architecture | Return to discovery |

---

## Common Issues

**CHANGES REQUIRED if:**
- Business value unclear
- Key stakeholders unknown
- Critical constraints missing
- Scope ambiguous
- Major unknowns exist

**APPROVED WITH NOTES if:**
- Minor clarifications needed
- Optional optimizations
- Future considerations

**APPROVED if:**
- All checklist items pass
- Ready for architecture

---

## Review Artifact

**Template:** `.opencode/templates/review-template.md`

**File:** `.artifacts/reviews/discovery-review-YYYY-MM-DD.md`

---

## Workflow Boundaries

**Entry:**
- Discovery complete
- Artifacts exist

**Exit (APPROVED):**
- Transition to architecture
- Delegate to @architect

**Exit (CHANGES REQUIRED):**
- Return to discovery
- Address gaps
- Re-submit for review

---

## Operating Principle

```
Validate understanding. Ensure clarity. Make gaps visible.
```
