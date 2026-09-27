# Improvement Processing Decision

**Date:** YYYY-MM-DD  
**Processed by:** ucn-consultant  
**Total improvements reviewed:** X

---

## Summary

**By Priority:**
- CRITICAL: X
- HIGH: X
- MEDIUM: X
- LOW: X

**By Category:**
- Context issues: X
- Workflow issues: X
- Template issues: X
- Permission issues: X
- Agent routing issues: X

**Decisions:**
- Implement: X
- Investigate: X
- Defer: X
- Dismiss: X

---

## Implemented

### [Improvement Title]

**File:** `.artifacts/improvements/YYYY-MM-DD-title.md`  
**Priority:** HIGH  
**Category:** Context  
**Issue:** Context discover failed to find X  
**Decision:** IMPLEMENT  
**Rationale:** Clear fix, high frequency (logged 3 times)

**Fix Applied:**
- Updated `.opencode/context/patterns/X.md`
- Added keywords: ["keyword1", "keyword2"]
- Validated: discover now finds file correctly

**Status:** ✅ Implemented and validated

---

### [Another Improvement]

...

---

## Deferred

### [Improvement Title]

**File:** `.artifacts/improvements/YYYY-MM-DD-title.md`  
**Priority:** MEDIUM  
**Category:** Template  
**Issue:** Template field not always needed  
**Decision:** DEFER  
**Rationale:** Low frequency (logged once), workaround exists, revisit in 1 month

**Status:** 📅 Deferred to 2026-10-27

---

## Dismissed

### [Improvement Title]

**File:** `.artifacts/improvements/YYYY-MM-DD-title.md`  
**Priority:** LOW  
**Category:** Formatting  
**Issue:** Prefer different markdown heading style  
**Decision:** DISMISS  
**Rationale:** Subjective preference, no impact on functionality, current style is standard

**Status:** ❌ Dismissed

---

## Requires Investigation

### [Improvement Title]

**File:** `.artifacts/improvements/YYYY-MM-DD-title.md`  
**Priority:** HIGH  
**Category:** Workflow  
**Issue:** Workflow step unclear, but root cause not obvious  
**Decision:** INVESTIGATE  
**Rationale:** Multiple agents logged confusion, but unclear how to fix without deeper analysis

**Investigation Plan:**
1. Review workflow execution logs
2. Interview agents (simulate workflow)
3. Identify exact point of confusion
4. Design fix
5. Re-submit for implementation

**Status:** 🔍 Investigation task created in `.artifacts/tasks/investigate-workflow-confusion.md`

---

## Metrics Update

**Before processing:**
- Total improvements logged: X
- Improvements implemented: Y
- Pending: Z

**After processing:**
- Improvements implemented: Y + [implemented count]
- Pending: Z - [processed count]
- Friction reduction: [estimated %]

---

## Next Steps

- Continue monitoring `.artifacts/improvements/` for new friction
- Re-visit deferred improvements: 2026-10-27
- Complete investigations and implement findings
- Run `/process-improvements` again when > 5 new improvements logged

---

*Generated from improvement-loop workflow*
