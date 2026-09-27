---
purpose: Transform approved milestone planning into working software
owner: developer
supports: [frontend, reviewer, architect, tester]
phase: build
artifacts:
  produce:
    - .artifacts/implementation/
    - .artifacts/testing/
    - .artifacts/reviews/
    - .artifacts/risks/
    - .artifacts/decisions/
  update:
    - .artifacts/planning/
    - .artifacts/tasks/
  read:
    - .artifacts/planning/milestones/
    - .artifacts/architecture/
    - PROJECT-CONFIG.md
entryFrom: [planning-review]
exitTo: [release, next-milestone, ucn-consultant]
commands: [/implement, /test, /review, /update-milestone, /create-risk, /create-decision]
avoid: [redesigning architecture, bypassing governance]
---

# Implementation Workflow

Transform approved milestone planning into working software following architecture, maintaining quality, updating progress continuously.

Implementation = one milestone at a time, one task per loop iteration.

---

## Sequence

Implementation follows **task loop** (`workflow/task-loop.md`) for each task:

```
PICK → DISCOVER → DO → CHECK → COMMIT → TRACK → LOOP → CONSULT
```

**High-level flow:**
1. Load context (architecture, tech stack, patterns)
2. Run task loop for each `[ ]` task in milestone
3. When all tasks `[x]` → delegate to `@ucn-consultant`
4. Consultant decides: more work or done

---

## Task Loop Execution

**For each task:**

1. **PICK** — Select first `[ ]` task from milestone
2. **DISCOVER** — Search existing code for reusable patterns (`context-manager.ps1 discover -Path "src/"`)
3. **DO** — Implement code + tests
4. **CHECK** — 4 gates:
   - BUILD (`build-verify` skill)
   - TEST (`run-tests` via @tester — automatic)
   - REVIEW (@reviewer — if `[high-risk]` tag)
   - SECURITY (@security — if `[security]` tag)
5. **COMMIT** — One commit per task (`git-commit` skill)
6. **TRACK** — Mark `[x]` in milestone, update task file with commit hash
7. **LOOP** — Return to step 1

**When all tasks `[x]`:**
- Delegate to `@ucn-consultant`
- Consultant reviews, decides next steps

See `workflow/task-loop.md` for detailed loop rules.

---

## Frontend & Backend Separation

**Backend tasks:** developer implements directly
**Frontend tasks:** delegate to `@frontend` (automatic)

Frontend delegation triggers when task:
- Mentions "component", "view", "UI", "frontend"
- Is in frontend-specific milestone
- Explicitly tagged `[frontend]`

`@frontend` agent:
- Loads Vue 3 / Nuxt context
- Implements component/view/service
- Follows existing patterns (reads `src/frontend/`)
- Returns when complete

---

## Context Loading

**Always load (startup):**
- `PROJECT-CONFIG.md` — tech stack and governance
- `.artifacts/planning/milestones/` — know all tasks
- `.opencode/context/techstack/overview.md` — tech overview

**Load per task type:**

| Task Type | Context Group | Command |
|-----------|---------------|---------|
| Service implementation | `services` | `context-manager.ps1 load -Group services -MaxTokens 4000` |
| Database work | `database` | `context-manager.ps1 load -Group database -MaxTokens 4000` |
| API endpoint (minimal) | `minimal-api` | `context-manager.ps1 load -Group minimal-api -MaxTokens 4000` |
| API endpoint (controller) | `api` | `context-manager.ps1 load -Group api -MaxTokens 4000` |
| Frontend | `frontend` | Delegate to @frontend |

**Discover existing code:**
```powershell
context-manager.ps1 discover -Task "[task description]" -Path "src/"
```

---

## Architecture Alignment

**3-Layer Architecture (standard):**
- **Api** — Controllers/endpoints, DTOs, validation, error handling
- **Business** — Services, business logic, orchestration
- **Data** — Repositories, database access, data models

**Rules:**
- Api → Business (allowed)
- Business → Data (allowed)
- Api → Data (forbidden — use Business layer)
- Business → Api (forbidden — inversion)

**When architecture is unclear:**
- Delegate to `@architect`
- Wait for architecture decision
- Resume implementation

---

## Governance by Track

Read `PROJECT-CONFIG.md` to determine governance level:

**Rapid track:**
- Skip optional reviews
- Minimal testing (build + basic tests)
- Fast iteration

**Production track:**
- Reviews via @reviewer (ask user)
- Standard testing (build + unit + integration)
- Security review for `[security]` tasks
- Comprehensive validation

---

## Workflow Boundaries

**Entry:**
- Planning review approved
- Milestones defined with tasks
- Architecture exists
- No blocking risks

**Exit:**
- All tasks in milestone `[x]`
- All commits pushed
- Milestone marked complete
- Delegate to `@ucn-consultant`

**May transition to:**
- Next milestone (if consultant creates more tasks)
- Release (if consultant confirms done)
- Architecture (if direction unclear)
- Risk escalation (if blocked)

**Must not:**
- Skip CHECK gates (build/test always required)
- Implement tasks from future milestones
- Change architecture without @architect
- Batch multiple tasks in one commit

---

## Progress Tracking

**Milestone file (`< .artifacts/planning/milestones/01-milestone-name.md`):**
```markdown
- [ ] Task 1
- [x] Task 2 (abc1234)  ← commit hash appended
- [~] Task 3 (blocked: needs API key)
```

**Task file (`.artifacts/tasks/task-name.md`):**
```yaml
status: completed
commit: abc1234
completed: 2026-09-27
```

**SESSION.md:**
```yaml
phase: build
milestone: 01-milestone-name
current task: Task 2
```

Update continuously — state is in files, not memory.

---

## Risk & Decision Handling

**When blocked:**
1. Mark task `[~]` in milestone
2. Create `.artifacts/risks/YYYY-MM-DD-risk-name.md`
3. Update SESSION.md blockers
4. Escalate to consultant or architect

**When architectural decision needed:**
1. Create `.artifacts/decisions/YYYY-MM-DD-decision-name.md`
2. Delegate to @architect if significant
3. Document decision before proceeding

---

## Testing Requirements

**Always:**
- Build must pass (BUILD gate)
- Tests must run (@tester automatic)

**By track:**
- **Rapid:** Minimal tests (smoke tests, critical paths)
- **Production:** Standard coverage (unit + integration, 70% target)

**Test structure:**
- Unit tests: `tests/UnitTests/`
- Integration tests: `tests/IntegrationTests/`
- Test per implementation file

See `context/architecture/testing-strategy.md`.

---

## Review Gates

**Automatic:**
- BUILD — always (`build-verify` skill)
- TEST — always (@tester)

**Conditional:**
- REVIEW — if task has `[high-risk]` tag
- SECURITY — if task has `[security]` tag

**Optional (ask user):**
- Implementation review (production track)
- Code quality review

Tags read from milestone file:
```markdown
- [ ] Create user authentication [security]
- [ ] Refactor database layer [high-risk]
```

---

## Operating Principle

```
One task at a time. One commit per task. Follow architecture. Write tests.
Discover before implementing. Track continuously. Escalate blockers.
Delegate when done.
```
