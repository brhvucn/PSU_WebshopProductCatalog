---
purpose: Validate architecture quality and implementation readiness
owner: reviewer (invoked by consultant)
supports: [architect, consultant]
phase: design
artifacts:
  produce:
    - .artifacts/reviews/architecture-review-YYYY-MM-DD.md
    - .artifacts/risks/ (if gaps found)
  read:
    - .artifacts/architecture/
    - .artifacts/discovery/
    - PROJECT-CONFIG.md
entryFrom: [architecture]
exitTo: [planning, architecture (if changes required)]
commands: [/review, /create-risk]
verdict: [APPROVED, APPROVED WITH NOTES, CHANGES REQUIRED]
---

# Architecture Review Workflow

Validate architecture quality, clarity, and readiness for planning.

---

## Review Checklist

**Architecture Clarity:**
- [ ] System boundaries defined
- [ ] Service/module responsibilities clear
- [ ] No overlapping ownership
- [ ] Integration points identified

**Tech Stack Alignment:**
- [ ] Aligns with PROJECT-CONFIG.md choices
- [ ] Frontend (Nuxt/Vue/none) addressed
- [ ] Backend (Minimal/Controller API) addressed
- [ ] Database (PostgreSQL/SQLite) addressed

**Architectural Decisions:**
- [ ] Monolith vs microservices decided (from intake)
- [ ] Communication patterns defined (sync/async)
- [ ] Data architecture clear
- [ ] Deployment strategy identified

**Documentation:**
- [ ] Architecture artifacts exist
- [ ] Domain model documented
- [ ] ADRs for significant decisions
- [ ] Integration architecture clear (if needed)

**Risks:**
- [ ] Architectural risks identified
- [ ] Scalability risks addressed
- [ ] Integration risks visible
- [ ] Mitigation strategies exist

**Simplicity:**
- [ ] Not over-engineered
- [ ] Standard patterns used
- [ ] Maintainability prioritized

---

## Verdict

| Verdict | Meaning | Next Step |
|---------|---------|-----------|
| **APPROVED** | Ready for planning | Transition to planning |
| **APPROVED WITH NOTES** | Proceed, refine later | Transition to planning, log notes |
| **CHANGES REQUIRED** | Fix before planning | Return to architecture |

---

## Common Issues

**CHANGES REQUIRED if:**
- System boundaries unclear
- Service responsibilities overlapping
- No alignment with PROJECT-CONFIG.md
- Missing architectural artifacts
- Over-engineered (premature optimization)
- Monolith/microservices not decided

**APPROVED WITH NOTES if:**
- Minor documentation gaps
- Optional optimization opportunities
- Future architectural considerations

**APPROVED if:**
- All checklist items pass
- Ready for planning

---

## Review Artifact

**Template:** `.opencode/templates/review-template.md`

**File:** `.artifacts/reviews/architecture-review-YYYY-MM-DD.md`

---

## Workflow Boundaries

**Entry:**
- Architecture defined
- Artifacts exist

**Exit (APPROVED):**
- Transition to planning
- Continue with consultant

**Exit (CHANGES REQUIRED):**
- Return to architecture
- Address gaps
- Re-submit for review

**Must not:**
- Approve unclear architecture
- Skip validation
- Modify artifacts directly (read-only)

---

## Operating Principle

```
Validate clarity. Ensure simplicity. Make gaps visible.
```
