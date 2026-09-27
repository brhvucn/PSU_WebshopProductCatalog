---
domain: project
capabilities:
  - tech-stack-overview
keywords:
  - tech stack
  - backend
  - frontend
  - architecture
  - .net 10
  - vue 3
  - 3-layer
  - services
priority: high
cost: 1
---

# Tech Stack — Overview

This is a **.NET 10 Microservices** demo project built with **Clean Architecture + CQRS** and **Controllers**.
Language: **C# 13**. All code and comments are written in **English**.

**Architecture Style:** Microservices — see `.artifacts/decisions/ADR-001-microservices-architecture.md`

## Backend

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Framework |
| Clean Architecture + CQRS | Architectural pattern |
| MediatR | Command/Query handling |
| Dapper + Entity Framework | Data access (mixed) |
| MSSQL | Database (Docker) |
| ASP.NET Web API | HTTP layer |
| FluentValidation | Input validation |

## Infrastructure

| Technology | Purpose |
|---|---|
| Docker Compose | Orchestration |
| Serilog | Logging |
| Seq | Log aggregation |
| Prometheus | Metrics collection |
| Smtp4Dev | Email testing |
| Swagger/OpenAPI | API documentation |

## Frontend

**Note:** This is primarily an API-focused demo. The Help service has a minimal UI for database seeding and overview.

No modern frontend framework - students interact via Swagger UI.

## Microservices

| Service | Port | Purpose |
|---------|------|---------|
| Catalog API | 8084 | Product catalog management |
| Customer API | 8085 | Customer CRUD operations |
| Payment API | 8083 | Payment simulation |
| Review API | 8086 | Product reviews |
| Help Service | 8000 | Database seed/reset + overview |

## Architecture

- **Clean Architecture:** Domain → Application → Persistence → API
- **CQRS:** Commands and Queries separated via MediatR
- **Controllers:** Traditional ASP.NET Web API controllers
- **Layers:** Controllers → MediatR Handlers → Repositories
- **Validation:** FluentValidation for request validation
- **Logging:** Serilog to Seq for centralized logging
- **Metrics:** Prometheus for monitoring

## Deployment

All services run via Docker Compose:

```powershell
docker-compose up
```

Students receive only the docker-compose.yml file and can run the entire demo locally.
