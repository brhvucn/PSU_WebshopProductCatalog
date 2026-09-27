---
domain: backend
capabilities:
  - patterns
  - database
keywords:
  - migration
  - schema
  - database migration
  - DbUp
  - versioning
  - rollback
  - idempotent
priority: high
cost: low
---

# Database Migration Pattern

Migrations evolve the database schema over time. Each migration is a versioned, append-only SQL script.

## Tool

Use **DbUp** — a lightweight .NET library that runs SQL scripts in order.

```bash
dotnet add package dbup-postgresql
```

## Naming Convention

```
V001__create_customers_table.sql
V002__add_email_to_customers.sql
V003__create_orders_table.sql
```

Format: `V{version}__{description}.sql`

- Version is zero-padded 3 digits
- Double underscore separates version from description
- Description uses `snake_case`

## Script Location

```
Infrastructure/Persistence/Migrations/
├── V001__create_customers_table.sql
├── V002__add_email_to_customers.sql
└── V003__create_orders_table.sql
```

## Execution

Migrations run at application startup via `IHostedService` or in `Program.cs`:

```csharp
var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(
        Assembly.GetExecutingAssembly(),
        s => s.Contains("Migrations"))
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
    throw new Exception("Migration failed", result.Error);
```

## Rules

- Migrations are **append-only** — never edit an existing migration
- Each migration must be **idempotent** where possible (`CREATE TABLE IF NOT EXISTS`, `ALTER TABLE ... ADD COLUMN IF NOT EXISTS`)
- Use **explicit transactions** per migration script
- Test migrations against a clean database before merging
- Never put seed data in migrations — use a separate `DbSeeder` class
- Never use EF Core migrations — use raw SQL via DbUp

## Rollback

For each migration, consider writing a corresponding rollback script:

```
Infrastructure/Persistence/Migrations/Rollback/
└── V002__rollback_add_email_to_customers.sql
```

Rollback scripts are manual — run them explicitly when needed.
Document rollback steps in the release artifact.

## Example Migration

```sql
-- V001__create_customers_table.sql
CREATE TABLE IF NOT EXISTS customers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    email VARCHAR(300) NOT NULL,
    created_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_customers_email ON customers (email);
```

## DI Registration

```csharp
// Infrastructure/DependencyInjection.cs
services.AddHostedService<DatabaseMigrationService>();
```

See also: `persistence.md`, `repository-template.md`.
