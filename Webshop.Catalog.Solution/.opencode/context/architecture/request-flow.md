---
domain: backend
capabilities:
  - architecture
keywords:
  - request flow
  - pipeline
  - services
priority: high
cost: low
---

# Request Flow

```txt
HTTP Request
    ↓
Controller (Api layer)
    ↓
Service Method Call (Business layer)
    ↓
  [Validation → Business Logic → Orchestration]
    ↓
Repository Interface Call (Data layer)
    ↓
Repository Implementation (Data layer)
    ↓
Dapper → PostgreSQL
    ↓
Data Model returned
    ↓
Service processes and returns result
    ↓
Controller maps to HTTP response
```

## Rules

- Controllers must not contain business logic — only HTTP concerns
- Services orchestrate business logic and coordinate repositories
- Services must not know concrete database implementation details
- Services use repository interfaces
- Repositories handle all database access
- Validation can happen in services or using validation attributes
- Cross-cutting concerns (logging, performance) handled via middleware or service decorators