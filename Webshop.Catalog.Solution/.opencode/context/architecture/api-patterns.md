---
domain: backend
capabilities:
  - architecture
keywords:
  - api
  - patterns
  - controllers
  - services
priority: high
cost: low
---

# API Patterns

The API layer handles HTTP concerns only. Controllers remain thin.

## Responsibilities

Controllers: receive HTTP requests, validate model binding, call service methods, map responses to HTTP results.

Controllers must NOT: contain business logic, contain SQL, or directly access repositories.

## Controller rules

- One controller per feature/entity where practical.
- Use constructor injection. Depend on service interfaces (e.g., `ITicketService`).
- Return typed HTTP responses where possible.
- Delegate all business logic to services.

## Incoming request

Incoming requests map to `*Request` objects (DTOs for incoming data).

Rules:
- `*Request` should not contain business logic.
- Located in `Api/DTOs/` or `Api/Models/`.
- Can use Data Annotations for basic validation.

## Outgoing DTO

Outgoing responses map to `*Dto` objects (DTOs for outgoing data).

Rules:
- `*Dto` should not contain business logic.
- Located in `Api/DTOs/` or `Api/Models/`.

`*Request` is incoming; `*Dto` is outgoing. Both are transport objects.

## Response handling

**Option 1: Exception-based**
- Services throw exceptions for errors
- Exception filter/middleware maps to HTTP status codes
- Controllers return Ok(), Created(), NoContent(), etc.

**Option 2: Result<T> pattern**
- Services return Result<T>
- Controllers check IsSuccess and map to HTTP status codes
- Return appropriate IActionResult based on result

## Error mapping

Handled through exceptions or Result<T> pattern (project choice).

## Versioning

Keep explicit, avoid breaking contracts, prefer additive changes.

## OpenAPI

Swagger/OpenAPI describes public endpoints. Controllers, Requests and DTOs should use descriptive names and response types.
