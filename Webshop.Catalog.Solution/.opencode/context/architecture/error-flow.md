---
domain: backend
capabilities:
  - architecture
keywords:
  - error flow
  - error handling
priority: high
cost: low
---

# Error Flow

```txt
Business failure   → Result.Failure(error) → Controller maps to ProblemDetails
Validation failure → ValidationException   → Validation behavior catches/maps to ProblemDetails
Infrastructure ex  → Exception             → Global middleware catches → ProblemDetails 500
```

## Rules

- Expected business failures should use `Result` / `Result<T>`.
- Validation failures should be handled consistently through validation behavior.
- Unexpected exceptions should bubble to global exception middleware.
- API responses should be mapped consistently to `ProblemDetails` where relevant.