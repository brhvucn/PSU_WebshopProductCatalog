---
domain: backend
capabilities:
  - patterns
keywords:
  - error handling
  - result
  - middleware
  - problem details
  - exception
  - rfc 7807
priority: medium
cost: low
---

# Error Handling

- Business rule violations → `Result.Failure(...)` — not exceptions.
- Infrastructure failures (DB down, network timeout) → exceptions caught by middleware.
- Global exception middleware in the **Api** layer translates unhandled exceptions to `ProblemDetails` (RFC 7807).
- Do not catch and swallow exceptions in handlers.

See also: `result-pattern.md`.
