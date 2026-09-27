# Frontend Implementation Rules

These rules apply **only when implementing frontend (Vue 3) functionality**.
For backend implementation, follow the architecture and patterns in `.opencode/context/`.

---

## Context Loading

Always discover frontend context before starting:

```powershell
.opencode\scripts\context-manager.ps1 discover -Task "frontend <task>" -MaxTokens 4000
```

Or load the frontend group directly:

```powershell
.opencode\scripts\context-manager.ps1 load -Group frontend -MaxTokens 6000
```

---

## Core Rule: Reuse Before Creating

Always inspect existing frontend implementations before writing new code.

The developer must:

- reuse existing page structure and component composition
- reuse existing naming conventions
- reuse existing spacing and layout patterns
- follow existing service patterns for API calls
- follow existing Pinia store patterns for state

The developer must not:

- introduce new layout styles without explicit user instruction
- create parallel component conventions
- duplicate existing UI patterns
- mix Bulma utility classes inconsistently

---

## File Locations

| Type | Location |
|---|---|
| Components | `src/frontend/components/` |
| Views / Pages | `src/frontend/views/` |
| Pinia stores | `src/frontend/stores/` |
| API services | `src/frontend/services/` |
| Router | `src/frontend/router/` |

---

## Patterns

See context files for concrete examples:

```powershell
.opencode\scripts\context-manager.ps1 load -Group examples -MaxTokens 4000
```

Key patterns:
- `context/frontend/service-pattern.md` — how to call the API
- `context/frontend/pinia-stores.md` — state management
- `context/frontend/conventions.md` — naming and structure
- `context/examples/` — real component and service examples
