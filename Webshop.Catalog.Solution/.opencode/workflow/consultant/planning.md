---
purpose: Transform validated architecture into milestone-based delivery plan
owner: consultant
supports: [architect, developer, reviewer]
phase: understand
artifacts:
  produce:
    - .artifacts/planning/
    - .artifacts/tasks/
    - .artifacts/risks/
    - .artifacts/decisions/
  read:
    - .artifacts/architecture/
    - .artifacts/discovery/
entryFrom: [architecture-review]
exitTo: [planning-review]
commands: [/plan, /generate-task-list, /create-task, /update-milestone, /create-risk, /create-decision]
avoid: [implementation, deployment]
---

# Planning Workflow

Transform validated architecture into incremental milestone-based delivery structure supporting agile execution, kanban prioritization, continuous refinement.

Avoid: rigid long-term prediction, waterfall planning.

---

## Sequence

```
/plan → define milestones → /generate-task-list → /create-task → 
/update-roadmap (optional) → /create-risk (optional) → /create-decision (optional) → 
planning-review
```

---

## Milestone-Based Planning

Each milestone = reviewable, deliverable solution increment.

**Structure:**
- 5-15 tasks per milestone
- Each task = independently implementable, testable, committable
- Sequence dependencies explicitly
- First milestone should deliver foundational infrastructure

**Criteria:**
- Clear goal (what value does this deliver?)
- Independently testable
- 1-2 weeks max duration
- No external blockers

**Avoid:**
- Milestones > 15 tasks (split them)
- Milestones < 3 tasks (merge them)
- Technical milestones without user value

---

## Task Structure

Each task must:
- Be independently implementable
- Be independently testable
- Produce one commit
- Fit in one task-loop iteration

**Format:**
```
- [ ] Description [tag]
```

**Tags:**
- `[security]` — triggers @security gate (auth, data access, endpoints)
- `[high-risk]` — triggers @reviewer gate (complex, cross-cutting)

**Example milestone:**
```markdown
# Milestone: User Authentication

- [ ] Create users table and repository [high-risk]
- [ ] Implement user service with password hashing [security]
- [ ] Create JWT token generation endpoint [security]
- [ ] Implement login endpoint [security]
- [ ] Add authentication middleware [security]
- [ ] Frontend: Login form component
- [ ] Frontend: Token storage and auth service
```

---

## Planning Principles

**Incremental:**
- Start small, grow iteratively
- Each milestone adds value
- Architecture emerges through delivery

**Agile:**
- Prioritize by value and risk
- Defer decisions until needed
- Adapt as understanding grows

**Kanban:**
- Work in progress limits (1 milestone at a time)
- Pull tasks as capacity allows
- Continuous flow over batch delivery

**Dependencies:**
- Make explicit: "depends on milestone X"
- Sequence to minimize blocking
- Flag external dependencies as risks

---

## Workflow Boundaries

**Entry:**
- Architecture review approved
- Architecture artifacts exist
- No blocking risks

**Exit:**
- Milestones defined (minimum 1, maximum 5 initially)
- All tasks structured and tagged
- Dependencies identified
- Roadmap created
- Planning review gate passed

**May transition to:**
- Planning review (normal flow)
- Architecture (if direction unclear)
- Discovery (if understanding insufficient)

**Must not:**
- Perform implementation
- Skip architecture review
- Create unbounded task lists
- Commit to long-term timelines

---

## Inputs

Read before planning:
- `.artifacts/architecture/` — what was designed
- `.artifacts/discovery/` — what problem is being solved
- `.artifacts/decisions/` — architectural constraints
- `.artifacts/risks/` — known blockers
- `context/architecture/` — architectural patterns
- `PROJECT-CONFIG.md` — project track and tech stack

---

## Review Gate

Planning review validates:
- [ ] Milestones are incremental and testable
- [ ] Tasks are right-sized (independently implementable)
- [ ] Dependencies identified and sequenced
- [ ] First milestone is foundational
- [ ] No unbounded scope
- [ ] Tags applied correctly ([security], [high-risk])

See `workflow/consultant/planning-review.md`.

---

## Feedback Loops

**If planning review identifies gaps:**
- Return to planning
- Refine milestones/tasks
- Re-submit for review

**If architecture is unclear:**
- Delegate to @architect
- Wait for architecture artifacts
- Resume planning

**If discovery is insufficient:**
- Return to discovery
- Clarify ambiguity
- Resume planning

---

## Runtime Updates

Update `.opencode/state/SESSION.md`:
- Set `phase: understand`
- Set `milestone: -` (planning phase, not executing milestones)
- Set `next: Planning review`

---

## Continuous Planning

Planning is not one-time. Throughout delivery:

**When developer finishes all tasks:**
- Consultant reviews milestone completion
- Decides: more work needed or done?
- Creates next milestone if needed

**When new requirements emerge:**
- Consultant creates new milestones/tasks
- Sequences appropriately
- Updates roadmap

**When tasks grow during implementation:**
- Consultant splits tasks
- Marks original `[~]` with "Split into sub-tasks"
- Creates new tasks

See task splitting criteria in `workflow/task-loop.md` and `agents/ucn-consultant.md`.

---

## Operating Principle

```
Plan incrementally. Prioritize by value. Defer decisions. Adapt continuously.
Never commit beyond the horizon. Make dependencies explicit. Keep tasks small.
```
