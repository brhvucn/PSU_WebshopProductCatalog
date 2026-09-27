# Instruction: Context Loading Discipline

## Purpose

The purpose of this instruction is to:

* minimize unnecessary context loading
* reduce token consumption
* improve agent focus and runtime stability
* prevent context explosion during execution

This instruction establishes mandatory runtime behavior for all agents.

---

# Core Workflow

Agents MUST follow this sequence before loading context:

1. Discover relevant context
2. Preview token cost
3. Load bounded context
4. Iterate only if necessary

---

# Required Commands

## Discover

Always begin with discovery:

```powershell
.opencode\scripts\context-manager.ps1 discover -Task "<task>"
```

---

## Preview

Before loading any group or large context:

```powershell
.opencode\scripts\context-manager.ps1 load -Group <name> -MaxTokens <n> -Preview
```

---

## Load

Only load bounded context:

```powershell
.opencode\scripts\context-manager.ps1 load -Group <name> -MaxTokens <n>
```

---

# Token Budgets

| Task Type              | Budget       |
| ---------------------- | ------------ |
| Small task             | 2.000        |
| Normal task            | 4.000–6.000  |
| Architecture / complex | 8.000        |
| Full audit             | Preview only |

Agents MUST prefer the smallest possible budget.

---

# Hard Rules

## Rule 1

Discover before loading.

---

## Rule 2

The following is FORBIDDEN:

```powershell
load -Group <name>
```

All group loads MUST include `-MaxTokens`.

---

## Rule 3

Always preview large groups before loading.

---

## Rule 4

Stop loading when context is sufficient.

---

## Rule 5

Do not load unrelated capability domains.

Example:

* backend tasks MUST NOT load frontend context
* frontend tasks MUST NOT load database context unless required

---

# Good Examples

```powershell
discover -Task "Create ticket service method"

load -Group services -MaxTokens 4000 -Preview

load -Group services -MaxTokens 4000
```

---

# Bad Examples

```powershell
load -Group frontend

load -Group architecture

load -Group cqrs
```

---

# Principle

Minimal relevant context is always preferred over complete context.