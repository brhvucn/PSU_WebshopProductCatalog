---
domain: backend
capabilities:
  - architecture
keywords:
  - layers
  - responsibilities
  - api
  - business
  - data
priority: high
cost: low
---

# Layer Responsibilities

## Api Layer (Presentation)

- Controllers, DTOs, HTTP concerns, middleware
- Receives HTTP requests, validates model binding
- Maps requests to service calls
- Maps service results to HTTP responses
- `Program.cs` wires dependencies via `AddBusiness()` + `AddData()` extension methods
- Global exception handling maps exceptions to `ProblemDetails` (if using exceptions)
- Controllers should be thin — no business logic

## Business Layer (Services)

- Services contain business logic, validation, orchestration
- Coordinates multiple repositories if needed
- Depends on Data layer abstractions (repository interfaces)
- Must not know about HTTP, controllers, or database implementation details
- Contains `DependencyInjection.cs` with `AddBusiness()` extension method
- Methods return results (exceptions or Result<T> pattern — project choice)
- Business rules, validation logic, workflows live here

## Data Layer (Repositories)

- Implements repository interfaces
- Contains Dapper queries, raw SQL, migrations, database access
- May reference database drivers (Npgsql, Dapper)
- Contains `DependencyInjection.cs` with `AddData()` extension method
- Database methods return results (exceptions or Result<T> pattern — project choice)
- Data models and entity mappings live here
