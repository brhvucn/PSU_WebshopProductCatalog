# Create Improvement Instruction

## Purpose

Log observations about the **agent system itself** so the system owner
can improve `.opencode/` files between sessions.

Improvements are NOT about the software project being built.
They are about how the agent system is configured and behaves.

---

## When to Execute

Execute this instruction when the agent system caused friction or blockage:

- An instruction in `.opencode/` was unclear or contradictory
- Agent routing was ambiguous — unclear which agent or phase owns a task
- `context-manager discover` failed to surface relevant files
- A template did not match actual usage
- A permission blocked something the agent should be allowed to do
- `shared-rules.md` or an agent file did not cover a situation clearly
- SESSION.md format was unclear to read or write

Do **not** create improvements for problems in the software project —
those belong in `.artifacts/risks/` or `.artifacts/decisions/`.

---

## Template

Always use `.opencode/templates/improvement-template.md`.
All fields are required. Fix Candidate must name a specific `.opencode/` file.

---

## File Naming

```
.artifacts/improvements/YYYY-MM-DD-short-title.md
```

Examples:
- `2026-05-22-discover-misses-mediatr-keywords.md`
- `2026-05-22-agent-routing-unclear-for-planning.md`
- `2026-05-22-shared-rules-missing-context-budget.md`

---

## After Creating

- If BLOCKER or HIGH: add to SESSION.md Blockers so the system owner sees it at next startup
- If the fix is a one-line change: fix it immediately and set status to `fixed`
- If the fix needs thought: leave as `open` — the system owner reviews `.artifacts/improvements/` periodically

---

## Completion Criteria

- File exists in `.artifacts/improvements/`
- Fix Candidate names at least one specific `.opencode/` file to change
- Status is `open` (or `fixed` if resolved immediately)
- SESSION.md updated if issue is blocking active work
