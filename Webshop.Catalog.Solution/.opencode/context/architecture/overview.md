---
domain: backend
capabilities:
  - architecture
keywords:
  - architecture
  - overview
  - 3-layer
  - services
priority: high
cost: low
---

# Architecture Overview

## Style

This project uses **3-layer architecture** with traditional ASP.NET Web API and standard **Controllers**.

**Architecture Style:** [monolith / microservices] — determined during intake phase.

Dependencies flow downward:

```txt
Api (Controllers, DTOs)
  ↓
Business (Services, Business Logic)
  ↓
Data (Repositories, Database Access)
```

Upper layers may depend on lower layers. Lower layers must never depend on upper layers.

## Architectural Principles

- **Api layer** contains controllers, DTOs, HTTP concerns, request/response mapping
- **Business layer** contains services, business logic, validation, orchestration
- **Data layer** contains repositories, database access, data models, queries
- Controllers should be thin — delegate to services
- Services orchestrate business logic and coordinate repositories
- Repositories handle all database access