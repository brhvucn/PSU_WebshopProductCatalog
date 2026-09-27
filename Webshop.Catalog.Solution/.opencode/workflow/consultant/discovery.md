---
purpose: Understand business value, stakeholders, constraints, and organizational context
owner: consultant
phase: understand
artifacts:
  produce:
    - .artifacts/discovery/business-value.md
    - .artifacts/discovery/stakeholders.md
    - .artifacts/discovery/constraints.md
    - .artifacts/risks/
  read:
    - .artifacts/clarification/
    - .artifacts/intake/
entryFrom: [clarification]
exitTo: [discovery-review]
commands: [/discover, /create-risk, /create-decision]
avoid: [architecture, planning, implementation]
---

# Discovery Workflow

Understand business value, stakeholders, organizational constraints, and context.

Transform clarified requirements into business understanding.

---

## Discovery Concerns

**Business Value:**
- What problem does this solve?
- Who benefits?
- What value is delivered?
- What are success criteria?
- How do we measure success?

**Stakeholders:**
- Who are the key stakeholders?
- Who makes decisions?
- Who uses the system?
- Who maintains the system?
- Who pays for it?

**Organizational Context:**
- What organizational constraints exist?
- What processes must be followed?
- What compliance requirements exist?
- What integration points exist?

**Technical Constraints:**
- What technical constraints exist?
- What existing systems integrate?
- What data sources exist?
- What infrastructure is available?

**Budget & Timeline:**
- What budget constraints exist?
- What timeline expectations exist?
- What are the priorities?
- What is MVP scope?

**Risks:**
- What business risks exist?
- What dependencies exist?
- What unknowns exist?
- What assumptions are we making?

---

## Sequence

```
Read clarification artifacts → Ask discovery questions →
Document business value → Identify stakeholders →
Document constraints → Identify risks → Discovery review
```

---

## Discovery Questions

**Business Value:**
- What business problem are you solving?
- What happens if you don't solve it?
- Who benefits from this solution?
- How do you measure success?

**Stakeholders:**
- Who are the primary users?
- Who are the decision makers?
- Who maintains the system?
- Who integrates with it?

**Constraints:**
- What technical constraints exist?
- What budget/timeline constraints?
- What organizational constraints?
- What compliance requirements?

**Scope:**
- What must be in v1 (MVP)?
- What can be deferred?
- What is out of scope?

**Integration:**
- What existing systems integrate?
- What data sources exist?
- What APIs are consumed?
- What APIs are provided?

---

## Artifacts

**Required:**
- `business-value.md` — problem, value, success criteria
- `stakeholders.md` — roles, responsibilities, contact
- `constraints.md` — technical, budget, timeline, organizational

**Optional:**
- `integration-context.md` — existing systems, APIs
- `scope.md` — in-scope, out-of-scope, MVP, future phases

**Templates:** `.opencode/templates/`

---

## Workflow Boundaries

**Entry:**
- Clarification complete
- Requirements clarified
- Ambiguity reduced

**Exit:**
- Business value clear
- Stakeholders identified
- Constraints understood
- Discovery review gate passed

**May transition to:**
- Discovery review (normal flow)
- Clarification (if new ambiguity found)
- Intake (if requirements fundamentally changed)

**Must not:**
- Define architecture
- Plan implementation
- Commit to technical decisions

---

## Operating Principle

```
Understand context. Identify value. Make constraints visible.
Support informed architecture and planning.
```
