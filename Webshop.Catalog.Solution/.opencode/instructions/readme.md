# Instructions

Instructions are focused procedures that agents execute for specific situations.
They are referenced from `shared-rules.md` and agent files — not loaded automatically.

## Structure

```
instructions/
  shared/                        <- Used by all agents
  consultant/                    <- Intake-specific rules
  developer/                     <- Implementation-specific rules
  create-improvement-command.md  <- Log agent system friction
```

## Instructions with Real Content

These contain rules that are not covered elsewhere:

| File | Contains | Used by |
|---|---|---|
| `shared/templates-reference.md` | All templates and when to use them | all agents |
| `shared/context-loading-discipline.md` | Hard rules for discover→preview→load | all agents |
| `shared/context-file-structure.md` | YAML metadata format, file size limits | all agents |
| `shared/create-task-instruction.md` | `[ ]` prefix rule, task structure | all agents |
| `consultant/intake-instruction.md` | `.intake/` scanning, Step 1/Step 2 | consultant |
| `developer/frontend-instructions.md` | Reuse existing patterns, no new layouts | developer |
| `create-improvement-command.md` | Improvement focus, naming, criteria | all agents |
