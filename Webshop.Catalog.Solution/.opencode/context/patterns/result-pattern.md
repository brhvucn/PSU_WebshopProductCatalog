---
domain: backend
capabilities:
  - patterns
keywords:
  - result pattern
  - result
  - error codes
  - result<t>
  - Result.Success
  - Result.Failure
priority: high
cost: low
---

# Result Pattern

- All Application layer operations return the project's **custom Result type** — never throw exceptions for business logic.
- Use `Result<T>` for operations that return a value, `Result` for void operations.
- Controllers map Result to HTTP responses — do not map Results inside handlers.
- Never use `Result.Success()` and throw exceptions in the same handler.
- Error codes live in static error classes in the **Domain** layer: `OrderErrors.cs`, `CustomerErrors.cs`.

See also: `error-handling.md`, `controller-template.md`.
