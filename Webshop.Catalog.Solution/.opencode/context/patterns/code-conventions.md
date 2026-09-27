---
domain: conventions
capabilities:
  - patterns
keywords:
  - code conventions
  - records
  - primary constructors
  - sealed
  - ireadonlylist
  - cancellationtoken
  - async/await
  - xml doc
priority: medium
cost: low
---

# Code Conventions

- Use **record** types for Commands, Queries, and DTOs — they are immutable by default.
- Use **primary constructors** where it reduces boilerplate (C# 12+).
- Use `sealed` on classes that are not designed for inheritance.
- Prefer `IReadOnlyList<T>` over `List<T>` in return types and DTOs.
- Always use `CancellationToken` in async methods — pass it through, never ignore it.
- Use `async/await` throughout — never `.Result` or `.Wait()`.
- Avoid `var` when the type is not immediately obvious from the right-hand side.
- Null checks use `ArgumentNullException.ThrowIfNull()` — not manual `if (x == null) throw`.
- All public API surface must have XML doc comments.

See also: `naming-conventions.md`.
