---
domain: backend
capabilities:
  - data-access
keywords:
  - database
  - postgresql
  - npgsql
  - persistence
  - migrations
priority: high
cost: 1
---

# Database Technology

- **PostgreSQL** is the primary database.
- Access via `NpgsqlConnection`.
- Data access via **Dapper** — see `patterns/repository-template.md` for usage patterns.
- Table names: `snake_case` and plural (`orders`, `order_lines`).
- Always use explicit column names in SELECT — never `SELECT *` in production code.
- Migrations are handled via scripts in `Infrastructure/Persistence/Migrations/` — not EF Core migrations.
- Configuration comes from environment variables and `appsettings.{Environment}.json`.

For data access patterns and repository templates, see `patterns/repository-template.md`.
