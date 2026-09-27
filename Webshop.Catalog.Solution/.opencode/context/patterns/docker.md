---
domain: conventions
capabilities:
  - patterns
keywords:
  - docker
  - docker compose
  - container
  - image
  - build
  - deployment
  - dockerfile
priority: medium
cost: low
---

# Docker

## Containerization

The backend .NET API and frontend Vue app can be containerized for local development and production.

## Docker Compose

```yaml
services:
  api:
    build:
      context: .
      dockerfile: src/Api/Dockerfile
    ports:
      - "5000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production

  frontend:
    build:
      context: .
      dockerfile: src/Frontend/Dockerfile
    ports:
      - "3000:80"
```

## Best Practices

- Use multi-stage builds for .NET to keep images small
- Never store secrets in Dockerfiles or Compose files — use environment variables or secret managers
- Tag images with both version and `latest`
- Always verify health checks after container starts
- Use liveness and readiness probes in production (Kubernetes)
- Pin base image versions — never use `latest` as base

## Health Checks

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:5000/health"]
  interval: 30s
  timeout: 10s
  retries: 3
```

See also: `health-checks.md`, `cloud-deployment.md`.
