---
purpose: Process logged agent system improvements and implement fixes
owner: ucn-consultant (manual trigger by user)
phase: meta
artifacts:
  produce:
    - .artifacts/improvements/implemented/
    - .artifacts/decisions/
  read:
    - .artifacts/improvements/
entryFrom: [user command /process-improvements]
exitTo: [implementation of framework fixes]
commands: [/process-improvements, /prioritize-improvement, /implement-improvement, /dismiss-improvement]
---

# Improvement Loop Workflow

Process friction logged in `.artifacts/improvements/` and implement fixes to agent system.

Triggered manually by user when improvements accumulate or system friction increases.

---

## Purpose

Agents log friction in `.artifacts/improvements/` but this workflow ACTS on those logs.

**Goal:** Continuous agent system improvement based on real usage friction.

---

## Workflow Sequence

### 1. Discovery

**Command:** `/process-improvements`

**Action:**
1. Read all files in `.artifacts/improvements/`
2. Group by category:
   - Context issues (discover failed, wrong files loaded)
   - Workflow issues (unclear steps, missing guidance)
   - Template issues (template doesn't match usage)
   - Permission issues (blocked action that should be allowed)
   - Agent routing issues (unclear which agent owns situation)
3. Count frequency of each issue
4. Identify patterns (same issue logged multiple times)

**Output:** Summary report showing:
- Total improvements: X
- By category: Context (X), Workflow (X), Template (X), etc.
- Top 5 most frequent issues
- Blocking vs non-blocking issues

---

### 2. Prioritization

**Criteria:**

| Priority | When | Example |
|----------|------|---------|
| **CRITICAL** | Blocks task completion | Permission denies required action, workflow has no exit |
| **HIGH** | Causes significant friction | Context discover misses critical files, template requires manual rework |
| **MEDIUM** | Minor friction, workaround exists | Instruction unclear but figure-out-able, template field not needed |
| **LOW** | Nice-to-have | Better naming, formatting improvements |

**Action:**
For each improvement file:
1. Read the improvement
2. Assess priority based on:
   - Frequency (logged multiple times = higher)
   - Impact (blocks work = critical)
   - Category (permission/workflow issues = higher than formatting)
3. Add priority tag to filename or move to priority folder

**Output:** Prioritized list

---

### 3. Decision

For each HIGH/CRITICAL improvement:

**Options:**

| Decision | When | Action |
|----------|------|--------|
| **IMPLEMENT** | Fix is clear, low risk | Schedule implementation |
| **INVESTIGATE** | Root cause unclear | Create investigation task |
| **DEFER** | Valid but low priority | Move to backlog |
| **DISMISS** | Not actually an issue | Document reason, archive |

**Create decision artifact:**
`.artifacts/decisions/improvement-decision-YYYY-MM-DD.md`

**Document:**
- Which improvements addressed
- Decision for each (implement/investigate/defer/dismiss)
- Rationale
- Implementation plan (if implement)

---

### 4. Implementation

**For each improvement to implement:**

1. **Identify fix location:**
   - Context file? (add keywords, update content)
   - Workflow file? (clarify step, add guidance)
   - Template? (update structure)
   - Permission? (update agent frontmatter)
   - Instruction? (add missing instruction)

2. **Make the fix:**
   - Edit the file directly
   - Test if possible (run context-manager discover with new keywords)
   - Document change

3. **Update improvement file:**
   - Add implementation section:
     ```markdown
     ## Implementation
     
     **Date:** YYYY-MM-DD
     **Fix:** Updated .opencode/context/patterns/X.md keywords
     **Changed:** Added "mediatr", "cqrs" to keywords list
     **Result:** Discover now finds CQRS patterns correctly
     ```

4. **Move to implemented:**
   - Move file to `.artifacts/improvements/implemented/`
   - Or mark as `[IMPLEMENTED]` in filename

5. **Update METRICS.md:**
   - Increment "Improvements implemented"
   - Update "Top friction points" (remove if fixed)

---

### 5. Validation

After implementing batch of improvements:

**Test:**
- Run through scenario that caused friction
- Verify fix works
- Check no regression

**Document:**
- Create validation report
- Add to implemented improvement file

---

## Improvement Categories & Typical Fixes

### Context Issues

**Symptom:** "discover failed to surface files it should have found"

**Typical fixes:**
- Add missing keywords to context file YAML frontmatter
- Create new context file for uncovered topic
- Add file to appropriate context group in groups.json

### Workflow Issues

**Symptom:** "instruction or rule unclear or contradictory"

**Typical fixes:**
- Clarify workflow step
- Add example to workflow
- Add decision tree or flowchart
- Update workflow boundaries (entry/exit criteria)

### Template Issues

**Symptom:** "template did not match actual usage"

**Typical fixes:**
- Update template structure
- Add/remove template fields
- Create new template variant
- Add template usage guidance

### Permission Issues

**Symptom:** "permission blocked something agent should be allowed to do"

**Typical fixes:**
- Update agent frontmatter permission block
- Add specific command to allowlist
- Document why permission exists (if block was correct)

### Agent Routing Issues

**Symptom:** "unclear which agent owns a situation"

**Typical fixes:**
- Update delegation.md agent responsibility matrix
- Clarify agent description
- Add routing decision tree to shared-rules.md

---

## Metrics

Track improvement loop effectiveness:

**In METRICS.md:**
- Improvements logged: X
- Improvements implemented: X
- Improvements dismissed: X
- Average time to implement: X days
- Friction reduction: (subjective but track)

---

## Example Improvement Processing

**Logged improvement:**
```markdown
# Context Discovery Missing MediatR Keywords

**Date:** 2026-05-22
**Agent:** ucn-developer
**Situation:** Implementing CQRS command handler
**Friction:** discover -Task "cqrs command handler" missed patterns/cqrs.md

**Root Cause:** patterns/cqrs.md missing "mediatr" keyword

**Fix Candidate:** Add "mediatr", "command", "query" keywords to patterns/cqrs.md frontmatter
```

**Processing:**
1. **Priority:** HIGH (causes context loading failure)
2. **Decision:** IMPLEMENT (fix is clear)
3. **Implementation:**
   - Edit `.opencode/context/patterns/cqrs.md`
   - Add keywords: `["cqrs", "mediatr", "command", "query", "handler"]`
4. **Validation:**
   - Run: `context-manager.ps1 discover -Task "mediatr command handler"`
   - Verify: patterns/cqrs.md now appears in results
5. **Mark implemented:**
   - Move to `.artifacts/improvements/implemented/2026-05-22-discover-misses-mediatr-keywords.md`
6. **Update METRICS.md:**
   - Improvements implemented: +1

---

## Workflow Boundaries

**Entry:**
- User runs `/process-improvements` command
- Improvements logged in `.artifacts/improvements/`

**Exit:**
- All HIGH/CRITICAL improvements processed
- Fixes implemented and validated
- Framework improved

**May transition to:**
- Back to normal development workflow

**Must not:**
- Skip validation
- Implement risky changes without testing
- Dismiss valid improvements without rationale

---

## Operating Principle

```
Learn from friction. Prioritize by impact. Implement systematically. Validate fixes.
Continuous improvement is a feature, not a bug.
```
