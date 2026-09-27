---
domain: backend
capabilities:
  - architecture
keywords:
  - solution structure
  - project layout
priority: high
cost: low
---

# Solution Structure

## Monolith Structure

```txt
src/
├── [SolutionName].Api/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Middleware/
│   └── Program.cs
├── [SolutionName].Business/
│   ├── Services/
│   │   └── [EntityName]Service.cs
│   ├── Interfaces/
│   │   └── I[EntityName]Service.cs
│   ├── Models/
│   │   └── BusinessModels/
│   ├── Validation/
│   └── DependencyInjection.cs
└── [SolutionName].Data/
    ├── Repositories/
    │   └── [EntityName]Repository.cs
    ├── Interfaces/
    │   └── I[EntityName]Repository.cs
    ├── Models/
    │   └── DataModels/
    ├── Database/
    │   └── Migrations/
    └── DependencyInjection.cs

tests/
├── UnitTests/
└── IntegrationTests/

docs/
├── architecture.md       <- architecture overview (synthesised by @documentation)
├── api.md                <- API documentation (synthesised by @documentation)
├── onboarding.md         <- developer onboarding guide (synthesised by @documentation)
└── decisions/
    └── ADR-NNN-title.md  <- architecture decision records (synthesised by @documentation)

README.md                 <- project readme (synthesised by @documentation)
docker-compose.yml
.env.example
```

## Microservices Structure

```txt
src/
├── Services/
│   ├── [ServiceName].Api/
│   │   ├── Controllers/
│   │   ├── DTOs/
│   │   ├── Middleware/
│   │   └── Program.cs
│   ├── [ServiceName].Business/
│   │   ├── Services/
│   │   ├── Interfaces/
│   │   ├── Models/
│   │   ├── Validation/
│   │   └── DependencyInjection.cs
│   └── [ServiceName].Data/
│       ├── Repositories/
│       ├── Interfaces/
│       ├── Models/
│       ├── Database/
│       └── DependencyInjection.cs
└── Shared/
    ├── [SolutionName].Shared.Contracts/
    │   └── DTOs/
    └── [SolutionName].Shared.Infrastructure/
        └── Common utilities

tests/
├── [ServiceName].UnitTests/
└── [ServiceName].IntegrationTests/

docs/
├── architecture.md
├── api.md
└── decisions/

README.md
docker-compose.yml
.env.example
```

## Notes
- **Monolith:** Single deployable application with 3 layers
- **Microservices:** Multiple independent services, each with 3 layers
- `Api` contains controllers, DTOs, middleware and startup wiring
- `Business` contains services, business logic, validation and orchestration
- `Data` contains repositories, database access and data models
- Each layer has `DependencyInjection.cs` for registering its services