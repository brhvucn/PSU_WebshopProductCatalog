---
domain: infrastructure
capabilities:
  - docker
keywords:
  - docker
  - docker-compose
  - environment
  - configuration
  - appsettings
  - deployment
  - health check
  - rollback
priority: medium
cost: low
---

# Docker

## Core Rules

- The project runs in Docker — do not hardcode connection strings or secrets.
- Configuration comes from environment variables and `appsettings.{Environment}.json`.
- `docker-compose.yml` is in the repo root — use it for local development.
- Never commit secrets or `.env` files — use `.env.example` as a template.

## docker-compose Structure

```yaml
services:
  api:
    build: .
    ports:
      - "5000:8080"
    environment:
      - DATABASE_URL=${DATABASE_URL}
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
    depends_on:
      db:
        condition: service_healthy

  db:
    image: postgres:16
    environment:
      - POSTGRES_DB=${POSTGRES_DB}
      - POSTGRES_USER=${POSTGRES_USER}
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER}"]
      interval: 5s
      timeout: 5s
      retries: 5
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

## Environment Variables

| Variable | Purpose | Example |
|---|---|---|
| `DATABASE_URL` | PostgreSQL connection string | `Host=db;Database=mydb;Username=user;Password=pass` |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Development` / `Production` |
| `POSTGRES_DB` | Database name | `mydb` |
| `POSTGRES_USER` | Database user | `user` |
| `POSTGRES_PASSWORD` | Database password | (use secrets) |

## Local Development

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop
docker-compose down

# Rebuild after code change
docker-compose up -d --build api
```

## Production Deployment

```bash
# Build image with version tag
docker build -t [project-name]:[version] .

# Deploy
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# Verify
docker-compose ps
docker-compose logs --tail=50 api
```

## Health Check

The API should expose a health endpoint:

```
GET /health  →  200 OK
```

Verify after deployment:

```bash
curl http://localhost:5000/health
```

## Rollback

```bash
# Roll back to previous version
docker-compose down
docker tag [project-name]:[previous-version] [project-name]:latest
docker-compose up -d
```
