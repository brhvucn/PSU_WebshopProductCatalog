# Create Task Instruction

## Purpose

The purpose of this instruction is to:

- create actionable implementation tasks
- preserve implementation visibility
- support incremental milestone delivery

This instruction should support:

```txt
structured and traceable implementation execution
```

---

## Templates

This instruction may use the following templates:

| Template | Purpose |
|---|---|
| `task-template.md` | Task documentation structure |
| `risk-template.md` | Related risk tracking |
| `decision-template.md` | Related implementation decision tracking |

Templates are optional and should only be used when structured artifacts are needed.

---

## Completion Criteria

This instruction is complete when:

- the task is documented
- the task uses `- [ ]` prefix so it can be tracked in the task loop
- task scope and dependencies are understandable
- the task is independently completable and testable (one loop iteration)
- milestone visibility is preserved
- implementation work can begin

## Task Loop Integration

Each task created by this instruction will be executed through the local loop defined in `workflow/task-loop.md`. Ensure tasks are:

- Small enough for one PICK → DO → CHECK → COMMIT → TRACK cycle
- Independently implementable without other tasks
- Able to produce a meaningful single commit
- Tracked with `[ ]` in the milestone file for checkbox state persistence
- Tagged appropriately: `[security]` for auth/data tasks, `[high-risk]` for complex tasks (triggers automatic @security/@reviewer gates in the task loop)