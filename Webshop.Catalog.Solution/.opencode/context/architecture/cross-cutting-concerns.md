---
domain: backend
capabilities:
  - architecture
keywords:
  - cross-cutting
  - middleware
  - logging
  - validation
priority: high
cost: low
---

# Cross-Cutting Concerns

Cross-cutting concerns are handled through middleware, service decorators, or attributes.

Services should focus on business logic orchestration.

## Mechanisms

| Mechanism | Responsibility | Where |
|---|---|---|
| Middleware | HTTP-level concerns (auth, logging, exceptions) | Api layer, Program.cs |
| Service Decorators | Service-level concerns (logging, caching, validation) | Business layer |
| Attributes | Declarative concerns (authorization, validation) | Controllers, methods |
| Action Filters | Request/response processing | Api layer |

## Rules

- Do not duplicate logging logic inside services.
- Keep cross-cutting logic centralized.
- Use middleware for HTTP-level concerns.
- Use decorators for service-level concerns.

## Validation

**Option 1: Data Annotations**
- Use `[Required]`, `[Range]`, etc. on DTOs
- Automatic validation via model binding

**Option 2: FluentValidation**
- Create validator classes
- Register in DI
- Use validation filter or manual validation in services

**Option 3: Manual validation**
- Validate in services
- Throw ValidationException or return Result<T> with errors

## Logging

Logging should include:

- request type
- execution duration
- failures
- correlation identifiers where relevant

Sensitive information must not be logged.

## Performance monitoring

Long-running handlers should be logged or flagged.

Performance thresholds should be configurable.

## Transactions

Where transactional consistency is required:

- transactions should be explicit
- transaction boundaries should remain predictable
- avoid hidden transaction scopes

## Authentication

Authentication is handled via JWT Bearer middleware — not pipeline behaviors.
Authorization uses `[Authorize]` attributes and named policies.
User identity is accessed via `ICurrentUserService` (Application layer abstraction).

See `patterns/authentication.md` for setup, claims handling and error mapping.

## Exception handling

Unhandled exceptions should bubble to global exception middleware.

Do not swallow unexpected exceptions silently.