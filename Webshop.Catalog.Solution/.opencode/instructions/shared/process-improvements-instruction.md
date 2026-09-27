# Process Improvements Instruction

**Command:** `/process-improvements`

**Owner:** ucn-consultant (manual trigger by user)

**Purpose:** Process logged agent system friction and implement fixes

---

## When to Use

Run this command when:
- Multiple improvements have accumulated (> 5 logged)
- System friction noticeably increasing
- Before starting major new project (clean up friction first)
- Monthly/quarterly maintenance cycle

---

## Execution

1. **Read all improvements:**
   ```powershell
   Get-ChildItem -Path ".artifacts/improvements" -Filter "*.md" | 
     Where-Object { $_.Directory.Name -ne "implemented" }
   ```

2. **Load improvement-loop workflow:**
   Read `.opencode/workflow/improvement-loop.md`

3. **Follow workflow:**
   - Discovery (categorize, count, identify patterns)
   - Prioritization (CRITICAL/HIGH/MEDIUM/LOW)
   - Decision (implement/investigate/defer/dismiss)
   - Implementation (make fixes)
   - Validation (test fixes)

4. **Update METRICS.md:**
   - Increment "Improvements implemented"
   - Update friction points

---

## Output

**Produce:**
- `.artifacts/decisions/improvement-decision-YYYY-MM-DD.md` — what was decided
- Updated framework files (context, workflows, templates, permissions)
- Moved files to `.artifacts/improvements/implemented/`

**Report to user:**
- Total improvements processed: X
- Implemented: X
- Deferred: X
- Dismissed: X
- Key fixes made

---

## Example

**User command:**
```
/process-improvements
```

**Agent response:**
```
Processing 12 logged improvements:

**By category:**
- Context issues: 5
- Workflow issues: 4
- Template issues: 2
- Permission issues: 1

**By priority:**
- CRITICAL: 1 (permission blocks required action)
- HIGH: 4 (context discover misses files)
- MEDIUM: 5
- LOW: 2

**Decisions:**
- Implementing: 5 (all CRITICAL/HIGH)
- Investigating: 2 (root cause unclear)
- Deferring: 3 (low priority)
- Dismissing: 2 (not actual issues)

Implementing fixes...

**Fixes made:**
1. Added "mediatr" keywords to patterns/cqrs.md
2. Clarified planning workflow step 3
3. Updated task-template.md with acceptance criteria field
4. Added /create-spike permission to ucn-developer
5. Created new context file for error-handling patterns

**Moved to implemented:** 5 files

Updated METRICS.md: Improvements implemented +5
```

---

## Rules

- Only ucn-consultant runs this command
- Validate all fixes before marking implemented
- Document decision rationale for deferred/dismissed improvements
- Update METRICS.md after processing
- Create improvement-decision artifact

---

## Related

- Workflow: `.opencode/workflow/improvement-loop.md`
- Template: `.opencode/templates/improvement-template.md`
- Metrics: `.opencode/state/METRICS.md`
