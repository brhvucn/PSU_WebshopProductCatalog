# [Project Name]

[One sentence: what this system does and who uses it.]

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker + docker-compose
- PostgreSQL (via Docker)

### Run locally

```bash
docker-compose up -d
dotnet run --project src/[Name].Api
```

The API is available at `http://localhost:[port]`.

---

## Project Structure

```
src/
  [Name].Api/           HTTP layer — controllers, middleware, startup
  [Name].Application/   Use cases — commands, queries, handlers, validators
  [Name].Domain/        Business rules — entities, value objects, errors
  [Name].Infrastructure/ Data access — Dapper repositories, migrations

tests/
  UnitTests/
  IntegrationTests/
```

---

## Architecture

3-layer architecture with service-oriented design.

```
Api → Business → Data
```

See `.artifacts/architecture/` for detailed architecture documents.

---

## Configuration

Copy `.env.example` to `.env` and fill in values:

```bash
cp .env.example .env
```

| Variable | Purpose |
|---|---|
| `DATABASE_URL` | PostgreSQL connection string |
| `[OTHER]` | [Purpose] |

---

## Development

```bash
# Run tests
dotnet test

# Build
dotnet build

# Apply migrations
[migration command]
```

---

## Deployment

```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

See `.artifacts/releases/` for release history.

---

## Documentation

| Document | Location |
|---|---|
| Architecture | `.artifacts/architecture/` |
| Decisions (ADRs) | `.artifacts/decisions/` |
| API documentation | `.artifacts/documentation/` |
