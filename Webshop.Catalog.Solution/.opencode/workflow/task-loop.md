# Task Loop Workflow

Local execution loop for each task in the active milestone. All agents MUST read this before task work.

## The Loop: PICK → DISCOVER → DO → CHECK → COMMIT → TRACK → LOOP → CONSULT

### Step 1: PICK
- Read `.artifacts/planning/milestones/` and `.artifacts/tasks/` — know every task and its status
- Select the first `- [ ]` task (skip `[x]` done, `[~]` blocked)
- If none remain → go to **Step 8: CONSULT**
- Update SESSION.md `current task`

### Step 2: DISCOVER (Code Reuse)

Before implementing, search for reusable code in existing project:

```powershell
# Search for similar implementations in src/
.opencode/scripts/context-manager.ps1 discover -Task "[task description]" -Path "src/"

# Load examples if found (within token budget)
.opencode/scripts/context-manager.ps1 load -Path "src/path/to/similar.cs" -MaxTokens 2000
```

**Decision tree:**
- **Exact match exists?** → Reuse directly (update if needed)
- **Similar pattern exists?** → Adapt pattern to new use case
- **No match?** → Implement new, follow existing patterns from context

**Benefits:** Consistency, reduced duplication, faster implementation, pattern discovery

**Rules:**
- Always discover before implementing new services/handlers/components
- Discovery budget: max 2000 tokens
- Document reused patterns in commit message

### Step 3: DO
- Load context per `instructions/shared/context-loading-discipline.md`
- Implement code + tests for this task only — never work ahead

### Step 4: CHECK

4 gates. Each can fail → return to Step 3 (DO/fix).

```
4a. BUILD    — load `build-verify` skill, execute (dotnet build / npm build)
4b. TEST     — @tester loads `run-tests` skill (automatic for ALL tasks)
4c. REVIEW   — @reviewer (only if task has [high-risk] tag)
4d. SECURITY — @security (only if task has [security] tag)
```

**Load a skill with:** `skill name="build-verify"`

**Gate failure handling:**

Each gate tracks failures. After 3 consecutive failures on ANY gate:

1. **Mark task blocked:** `[~]` in milestone with reason
2. **Create risk artifact:** `.artifacts/risks/YYYY-MM-DD-gate-failure-[task-name].md`
3. **Update METRICS.md:** Increment gate failure counter
4. **Update SESSION.md blockers:** Add blocker with gate name
5. **Escalate:** Delegate to @ucn-consultant for resolution

**Risk artifact must include:**
- Which gate failed (BUILD/TEST/REVIEW/SECURITY)
- Error messages / failure reasons
- Attempted fixes (what was tried)
- Recommended action (escalate to architect, need external resource, etc.)

**Recovery options:**

| Scenario | Action |
|----------|--------|
| BUILD fails 3x | Likely architecture issue → escalate to @architect |
| TEST fails 3x | Likely design issue → escalate to @architect or @reviewer |
| REVIEW fails 3x | Likely scope issue → escalate to @ucn-consultant (task too large?) |
| SECURITY fails 3x | Likely design issue → escalate to @architect + @security |

**Automatic recovery (before 3 failures):**
- Failure 1 → Retry with context reload
- Failure 2 → Search for similar solved issues in `.artifacts/risks/` (learn from past)
- Failure 3 → Escalate (as above)

Read task tags from the milestone file line: `- [ ] Implement login [security]`. If tag present, invoke the subagent. Wait for its verdict. Only pass if verdict is PASS. If any gate fails → fix and retry.

### Step 5: COMMIT
- Load `git-commit` skill: `skill name="git-commit"`
- Execute the instructions — stages all changes and creates one conventional commit
- If frontend-only, delegate to `@frontend` (frontend loads `git-commit` itself)

### Step 6: TRACK
- Milestone: change `- [ ]` → `- [x]` and append commit hash: `- [x] Task (abc1234)`
- Task file (`.artifacts/tasks/<name>.md`): update status, add commit hash + date
- Append entry to `.artifacts/planning/milestones/<name>/progress.md`

### Step 7: LOOP
- Return to Step 1. Read milestone/task files fresh. Pick next `- [ ]`.

### Step 8: CONSULT
When ALL tasks in milestone are `[x]`:
1. Mark milestone complete, update SESSION.md: `next: Review with @ucn-consultant`
2. **Delegate to `@ucn-consultant`** — consultant decides:
   - More work? Creates new milestones/tasks → return to Step 1
   - Project done? Emits `<promise>All milestones complete.</promise>` — only consultant emits this

## Startup Sequence (every session)
1. Load `git-init` skill: `skill name="git-init"` — ensures a git repo exists
2. Read ALL milestone + task files — know project state from files, not memory
3. Read SESSION.md
4. If `[ ]` tasks exist → continue loop
5. If ALL tasks `[x]` → delegate to `@ucn-consultant`
6. If no files exist → start with `ucn-consultant`

## Rules
- **One task at a time** — never parallel
- **One commit per task** — never batch
- **4 gates before commit** — BUILD(`build-verify`), TEST(`run-tests` via @tester), REVIEW(@reviewer if [high-risk]), SECURITY(@security if [security])
- **Load skills via `skill` tool** — never inline commands
- **Persist in files** — state is `[ ]`/`[x]` in milestone + task files, not memory
- **Read all tasks on startup** — always know where the project stands
- **Idempotent** — reading files is always sufficient to resume
- **Blocked tasks** — mark `[~]`, log risk, escalate
- **No tasks → @ucn-consultant** — only consultant decides next steps or completion

## Task Creation Rules (for consultant)
- Each task = one loop iteration: independently implementable, testable, committable
- Format: `- [ ] Description [tag]` — add `[security]` for auth/data tasks, `[high-risk]` for complex tasks
- Prefix files numerically: `01-setup-auth.md`
- Too large? Split. E.g. `- [ ] Implement user management` → `- [ ] Create DB table [high-risk]`, `- [ ] Service + endpoint [security]`, `- [ ] JWT login [security]`

## Routing
- **Milestone done** → delegate to `@ucn-consultant`
- **Consultant creates tasks** → continue loop from Step 1
- **Consultant confirms done** → emit completion promise, end
- **Blocked** → escalate, do not start new tasks
- **Architecture question** → delegate to `@architect`, await resolution
- **Frontend work** → delegate to `@frontend`, await return
