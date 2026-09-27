---
domain: backend
capabilities:
  - architecture
keywords:
  - persistence
  - dapper
  - postgresql
priority: high
cost: low
---

# Persistence

Persistence is implemented in Infrastructure.

The project primarily uses Dapper with PostgreSQL.

## Rules

- Infrastructure owns persistence concerns.
- Application must depend on abstractions only.
- Repositories implement interfaces defined by inner layers.
- SQL should remain explicit and readable.
- Avoid hidden ORM behavior.

## Repository rules

Repositories should:

- encapsulate SQL access
- return domain entities or DTOs
- remain focused on persistence concerns

Repositories should not:

- contain business orchestration
- contain HTTP concerns
- know about controllers

## Dapper usage

Rules:

- Prefer explicit SQL.
- Use parameterized queries.
- Avoid string concatenation for SQL parameters.
- Prefer multi-line raw string SQL where possible.

Example:

```csharp
const string sql = """
SELECT
    id,
    name
FROM customers
WHERE id = @Id
""";
```

## Database access

- PostgreSQL is the primary database.
- Use NpgsqlConnection.
- Connections should be short-lived.
- Transactions should be explicit where required.

## Migrations

Database scripts and migrations live in:

```txt
Infrastructure/Persistence/Database/
Infrastructure/Persistence/Migrations/
```

Migrations use **DbUp** with versioned SQL scripts. See `patterns/migration-pattern.md` for naming conventions, execution strategy and rollback approach.

## Query Separation

Complex read queries may use dedicated query repositories or projections.

Do not force aggregates into inefficient read patterns.