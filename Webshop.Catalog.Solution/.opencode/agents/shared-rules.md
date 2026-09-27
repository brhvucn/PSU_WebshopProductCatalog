# Shared Agent Rules

These rules apply to all agents in this system.
Reference this file with: `See shared-rules.md`

---

## Startup

1. **Read `.opencode/state/PROJECT-CONFIG.md`** — know project track (rapid/production), tech stack, governance rules (including dynamic adjustments)
2. **Read `.opencode/state/METRICS.md`** — know agent performance, bottlenecks, health indicators
3. **Read ALL milestone files** in `.artifacts/planning/milestones/` — know every milestone, its tasks and their `[ ]`/`[x]` status
4. **Read ALL task files** in `.artifacts/tasks/` — understand every task, completed and pending
5. Read `.opencode/state/SESSION.md` — this tells you the current phase, milestone, and next step
6. Check `.artifacts/` for artifacts relevant to the current task
7. **Ensure a git repository exists** — load `git-init` skill: `skill name="git-init"`
8. Read `workflow/task-loop.md` — this defines the local task execution loop for every individual task
9. Based on the state you read:
   - If `[ ]` tasks exist, continue where you left off
   - If ALL tasks are `[x]` and no new milestones, delegate to `@ucn-consultant`
   - If no milestone/task files exist, start with `ucn-consultant` for intake
10. Continue from persisted state — never restart from scratch

---

## Context Loading

**Prerequisite:** PowerShell Core (pwsh 7+) is required for cross-platform support.
Install: https://learn.microsoft.com/powershell/scripting/install/installing-powershell

Always follow this sequence:

```powershell
# 1. Discover what is relevant
.opencode/scripts/context-manager.ps1 discover -Task "<task>" -MaxTokens 4000

# 2. Preview cost before loading
.opencode/scripts/context-manager.ps1 load -Group <name> -MaxTokens <n> -Preview

# 3. Load only what is needed
.opencode/scripts/context-manager.ps1 load -Group <name> -MaxTokens <n>
```

**Token budgets:**

| Task | Budget |
|---|---|
| Small / focused | 2 000 |
| Normal | 4 000–6 000 |
| Architecture / complex | 8 000 |
| Audit / overview | Preview only |

**Hard rules:**
- Always discover before loading
- Never `load -Group <name>` without `-MaxTokens`
- Load the minimum — add more only if needed
- Do not load unrelated domains (backend tasks must not load frontend context)

---

## State

Update `.opencode/state/SESSION.md` when:
- Phase changes (understand → build → review → release)
- Milestone changes or completes
- A significant blocker appears or is resolved
- The next recommended step changes

Update `.opencode/state/METRICS.md` when:
- Task completes (increment completed counter, update duration)
- Task fails/blocks (increment failed/blocked counter)
- Gate executes (increment gate counter, record pass/fail)
- Agent invoked (increment invocation counter, track duration)
- Code reuse discovered (increment reuse counter)
- Improvement logged (increment improvement counter)

Keep SESSION.md short and current — it is the single source of truth for session continuity.
Persist decisions, risks and artifacts to `.artifacts/` for durable history.
Never rely on chat memory for continuity.

METRICS.md tracks performance — update automatically during task loop execution.

---

## Governance

- Follow the active workflow — do not skip phases
- Make incomplete work visible — never hide it
- Do not silently change architecture or planning
- Prefer visible uncertainty over false confidence
- Follow the git workflow — see `context/patterns/git-workflow.md` for branching, commits and PRs

---

## Improvement

Improvements are observations about the **agent system itself** — not the software project.
When `.opencode/` configuration caused friction or blockage, log it so the system owner can fix it.

Log immediately when:

- An instruction or rule in `.opencode/` was unclear or contradictory
- `context-manager discover` failed to surface files it should have found
- Agent routing was ambiguous — unclear which agent owns a situation
- A template did not match actual usage
- A permission blocked something the agent should be allowed to do

**How:**
1. Create `.artifacts/improvements/YYYY-MM-DD-short-title.md`
2. Use `.opencode/templates/improvement-template.md` — all fields required
3. Fix Candidate must name the specific `.opencode/` file to change
4. If blocking: update SESSION.md Blockers

Do not log problems in the software project here — those go in `.artifacts/risks/`.

---

## Context Maintenance

When new knowledge is discovered during a session that should be reusable:

```powershell
# Stage new knowledge for review
.opencode\scripts\context-manager.ps1 harvest -Source "<file-or-text>" -Target <category>
```

When a context file exceeds ~300 lines or feels verbose:

```powershell
# Preview what would be removed
.opencode\scripts\context-manager.ps1 compress -Path "<context-file>" -Preview

# Apply compression
.opencode\scripts\context-manager.ps1 compress -Path "<context-file>"
```

Harvested files land in `.opencode/context/.harvest/<category>/` for human review
before being promoted to the curated context folders.

---

## Shared Instructions

Use these instructions when the situation calls for them.

| Instruction | When to use |
|---|---|
| `instructions/shared/templates-reference.md` | Find the right template for any artifact or document |
| `instructions/shared/context-loading-discipline.md` | Reference for discover→preview→load rules |
| `instructions/shared/context-file-structure.md` | Reference for YAML metadata format in context files |
| `instructions/shared/create-task-instruction.md` | Creating a new tracked task with `[ ]` prefix |
| `instructions/consultant/intake-instruction.md` | `.intake/` scanning before asking the user |
| `instructions/developer/frontend-instructions.md` | Frontend implementation — reuse existing patterns |
| `instructions/create-improvement-command.md` | Logging agent system friction |
| `workflow/task-loop.md` | **Local task execution loop** — read before any task work |
| `skills/build-verify/SKILL.md` | **Build verify skill** — load via `skill name="build-verify"` after code changes |
| `skills/run-tests/SKILL.md` | **Run tests skill** — load via `skill name="run-tests"` after build passes |
| `skills/git-commit/SKILL.md` | **Git commit skill** — load via `skill name="git-commit"` after verification gates pass |
| `skills/git-init/SKILL.md` | **Git init skill** — load via `skill name="git-init"` at session startup |

---

## Output Style

**Concise and action-oriented:**
- Show what you're doing, not explanations of plans
- Avoid long recaps, summaries, preambles
- Code and artifacts speak for themselves — explain only when critical ambiguity exists
- Get to the point immediately

**Structure:**
- Incremental, clear
- Explicit assumptions and unknowns
- Actionable next steps
- No speculation as fact

**Avoid:**
- Multi-paragraph summaries before action
- Explaining each step before and after doing it
- Verbose "let me explain" sections
- Repetitive confirmations of what user already knows
