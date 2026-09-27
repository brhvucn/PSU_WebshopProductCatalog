# Intake Instruction

## Purpose

The purpose of this instruction is to:

- capture raw stakeholder input
- preserve original intent
- initialize structured discovery work

This instruction should support:

```txt
accurate and low-friction intake before clarification
```

---

## Step 1: Scan `.intake/` before asking the user

Before asking the user for any input, the agent MUST:

```txt
List all files in .opencode/.intake/
that are NOT inside processed/ or archived/
```

**If unprocessed files exist:**

- Show the user a numbered list of files (name + approximate size)
- Ask: *"I found [N] file(s) in .intake/. Should I process them, or do you want to describe something new?"*
- Use the selected file's content as the raw input — do not ask the user to re-describe it
- After creating the intake artifact, move the file to `.opencode/.intake/processed/`

**If no unprocessed files exist:**

- Ask the user to describe the new work directly

This rule exists to avoid asking the user to repeat information they have already written down.

---

## Step 2: Capture and preserve raw input

Whether the source is a `.intake/` file or direct user input:

- Preserve the original wording exactly
- Do not interpret, summarize or rewrite at this stage
- Record the source type (file name, or "direct input")

---

## Templates

This instruction may use the following templates:

| Template | Purpose |
|---|---|
| `discovery/problem-statement-template.md` | Initial problem understanding |
| `discovery/stakeholder-template.md` | Stakeholder identification |
| `discovery/business-value-template.md` | Initial business value understanding |

Templates are optional and should only be used when structured artifacts are needed.

---

## Completion Criteria

This instruction is complete when:

- `.intake/` has been scanned and any pending files have been handled
- the original request has been preserved
- initial ambiguity has been identified
- initial intake artifacts have been created in `.artifacts/intake/`
- processed source files have been moved to `.opencode/.intake/processed/`
- clarification work can begin