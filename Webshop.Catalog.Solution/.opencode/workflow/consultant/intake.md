---
purpose: Capture raw requirements, preserve original wording, identify obvious gaps
owner: consultant
phase: understand
artifacts:
  produce:
    - .artifacts/intake/intake-YYYY-MM-DD.md
    - .artifacts/risks/ (if obvious blockers)
  read:
    - .opencode/.intake/ (if files exist)
entryFrom: [user, project-init]
exitTo: [clarification]
commands: [/intake, /create-risk]
avoid: [clarification, architecture, planning, implementation]
---

# Intake Workflow

Capture raw requirements exactly as provided. Preserve original wording. Identify obvious gaps.

First phase after project initialization.

---

## Intake Sources

**Check `.opencode/.intake/` folder:**
- List files NOT in `processed/` or `archived/`
- If found: show list, ask which to process
- Use file content as raw input
- Move processed files to `.opencode/.intake/processed/`

**If no files:**
- Ask user to describe requirements
- Capture raw input verbatim

---

## Intake Process

**Capture:**
- What does the user want?
- Original wording (verbatim)
- Any provided context
- Any provided constraints

**Identify obvious gaps:**
- Missing critical information
- Unclear terminology
- Ambiguous scope
- Missing stakeholders
- Missing constraints

**Do NOT:**
- Clarify ambiguity (that's clarification phase)
- Interpret requirements
- Make assumptions
- Plan architecture
- Define scope

---

## Architecture Style Decision

During intake, determine architecture style (if not already specified):

**Check:**
1. `PROJECT-CONFIG.md` — already decided?
2. Intake files — user mention monolith or microservices?
3. Existing artifacts — ADR exists?

**If not specified, ask user:**

```
Should this project be:
1. Monolith (single deployable, simpler, good for small-medium projects)
2. Microservices (multiple services, complex, good for large projects)
```

**Record decision:**
Create ADR: `.artifacts/decisions/ADR-001-monolith-vs-microservices.md`

---

## Sequence

```
Check .opencode/.intake/ → Process files OR ask user →
Capture raw requirements → Identify obvious gaps →
Decide monolith vs microservices → Transition to clarification
```

---

## Artifacts

**Required:**
- `intake-YYYY-MM-DD.md` — raw requirements verbatim

**Template:** `.opencode/templates/intake-raw-template.md`

---

## Workflow Boundaries

**Entry:**
- Project initialized (PROJECT-CONFIG.md exists)
- User provides requirements

**Exit:**
- Raw requirements captured
- Monolith/microservices decided
- Transition to clarification

**May transition to:**
- Clarification (normal flow)
- Project init (if PROJECT-CONFIG not configured)

**Must not:**
- Interpret requirements
- Make assumptions
- Plan architecture
- Define scope

---

## Operating Principle

```
Capture raw. Preserve original. Identify gaps. Defer interpretation.
```
