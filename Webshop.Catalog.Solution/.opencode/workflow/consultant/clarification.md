---
purpose: Reduce ambiguity, identify business value, surface assumptions
owner: consultant
phase: understand
artifacts:
  produce:
    - .artifacts/clarification/clarification-YYYY-MM-DD.md
    - .artifacts/risks/ (if blockers found)
  read:
    - .artifacts/intake/
entryFrom: [intake, discovery (if new ambiguity)]
exitTo: [discovery]
commands: [/clarify, /create-risk]
avoid: [architecture, planning, implementation]
---

# Clarification Workflow

Reduce ambiguity in requirements. Ask questions. Surface assumptions. Identify business value.

Transform raw intake into clarified requirements.

---

## Clarification Process

**Read intake artifacts:**
- What did user request?
- What is ambiguous?
- What is missing?
- What assumptions exist?

**Ask clarifying questions:**
- What is unclear?
- What terminology is ambiguous?
- What scope is uncertain?
- What priorities are unclear?

**Document clarified requirements:**
- Resolved ambiguity
- Clear terminology
- Defined scope
- Explicit assumptions
- Initial value statement

**Identify risks:**
- Unclear requirements
- Conflicting priorities
- Missing information
- Dependencies

---

## Clarification Questions

**Scope:**
- What features are essential?
- What features are optional?
- What is out of scope?

**Terminology:**
- What does [term] mean in your context?
- What is the difference between [A] and [B]?

**Priorities:**
- What must be in v1?
- What can be deferred?
- What is most valuable?

**Users:**
- Who will use this?
- What are their needs?
- What problems do they have?

**Success:**
- What does success look like?
- How do you measure it?
- What are the acceptance criteria?

---

## Sequence

```
Read intake → Identify ambiguity → Ask questions →
Document clarified requirements → Surface assumptions →
Transition to discovery
```

---

## Artifacts

**Required:**
- `clarification-YYYY-MM-DD.md` — resolved ambiguity, clarified requirements

**Template:** `.opencode/templates/clarification-template.md`

---

## Workflow Boundaries

**Entry:**
- Intake complete
- Raw requirements exist

**Exit:**
- Ambiguity reduced
- Requirements clarified
- Assumptions visible
- Transition to discovery

**May transition to:**
- Discovery (normal flow)
- Intake (if requirements fundamentally changed)

**Must not:**
- Define architecture
- Plan implementation
- Commit to technical decisions

---

## Operating Principle

```
Ask questions. Reduce ambiguity. Surface assumptions. Clarify before discovering.
```
