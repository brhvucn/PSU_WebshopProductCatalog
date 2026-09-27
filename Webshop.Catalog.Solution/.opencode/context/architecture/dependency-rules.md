---
domain: backend
capabilities:
  - architecture
keywords:
  - dependency
  - layering
  - architecture
  - 3-layer
priority: high
cost: low
---

# Dependency Rules

Downward flow: `Api→Business→Data`. Never upward.

---

# Dependencies

- **Api** → Business (NOT Data directly)
- **Business** → Data (via interfaces only)
- **Data** → None (independent layer)

---

# Principles

**Api is presentation** — HTTP concerns only. Controllers, DTOs, middleware, auth, exception handling, request/response mapping. No business logic or data access.

**Business is orchestration** — services contain business logic, validation, workflows. Depends on repository interfaces. No HTTP or database implementation details.

**Data is persistence** — repositories implement data access. SQL, Dapper, migrations, database concerns. No business logic or HTTP concerns.

---

# Interface Placement

| Type                          | Location                         |
| ----------------------------- | -------------------------------- |
| Repository interfaces          | Data.Interfaces or Business.Interfaces |
| Service interfaces             | Business.Interfaces              |
| Repository implementations     | Data.Repositories                |
| Service implementations        | Business.Services                |

# DTO Rules

DTOs in Api layer. Used for HTTP transport only. Business layer uses business models. Data layer uses data models. Keep layers separated.

# Anti-Patterns

SQL in controllers, business logic in controllers, controllers calling repositories directly, circular dependencies, fat controllers, fat repositories with business logic.

# Enforcement

Depend on abstractions. Business rules in Business layer, HTTP in Api layer, persistence in Data layer. Prefer explicit boundaries.
