# Developer Onboarding: [Project Name]

*For new developers joining the project.*
*Synthesised from `.artifacts/` and project source.*
*Last updated: [YYYY-MM-DD]*

---

## What This System Does

```txt
[One paragraph. Source: .artifacts/discovery/]
```

---

## Prerequisites

- .NET 10 SDK
- Docker Desktop
- Git
- [IDE — Rider / VS Code / Visual Studio]

---

## First Run

```bash
# Clone
git clone [repo-url]
cd [project-name]

# Configure environment
cp .env.example .env
# Edit .env with your local values

# Start dependencies
docker-compose up -d

# Run the API
dotnet run --project src/[Name].Api

# Run tests
dotnet test
```

The API is available at `http://localhost:[port]`.

---

## Project Structure

```
src/
  [Name].Api/        Controllers, DTOs, HTTP layer
  [Name].Business/   Services, business logic
  [Name].Data/       Repositories, database access

tests/
  UnitTests/
  IntegrationTests/

.opencode/           Agent system configuration
.artifacts/          Delivery history — decisions, architecture, planning
```

---

## Key Concepts

**3-Layer Architecture**
Dependencies flow downward: `Api → Business → Data`. Controllers delegate to services. Services orchestrate business logic and coordinate repositories.

**Service-Oriented Design**
Business logic lives in services. Services are injected into controllers via interfaces.

**Repository Pattern**
All database access through repositories. Repositories implement interfaces defined in Data layer.

**Error Handling**
Use exceptions or Result<T> pattern (project choice). Controllers map errors to HTTP status codes.

See `docs/architecture.md` for the full architecture overview.

---

## Where to Find Things

| I want to... | Look in... |
|---|---|
| Add a new feature | `src/[Name].Application/Features/[Entity]/` |
| Add a new endpoint | `src/[Name].Api/Controllers/` |
| Add a domain rule | `src/[Name].Domain/Entities/` |
| Add a repository | `src/[Name].Infrastructure/Persistence/Repositories/` |
| Understand a decision | `docs/decisions/` or `.artifacts/decisions/` |
| See the roadmap | `.artifacts/planning/` |

---

## Development Workflow

1. Check `SESSION.md` in `.opencode/state/` for active milestone
2. Pick a task from `.artifacts/tasks/`
3. Implement in the relevant feature folder
4. Write tests alongside implementation
5. Update task status when done

---

## Contacts and Resources

| Resource | Location |
|---|---|
| Architecture overview | `docs/architecture.md` |
| API documentation | `docs/api.md` |
| Decision log | `docs/decisions/` |
| Release history | `.artifacts/releases/` |

---

<!-- TODO: verify run instructions against actual docker-compose.yml -->
