---
domain: infrastructure
capabilities:
  - cicd
  - deployment
keywords:
  - github actions
  - ci
  - cd
  - pipeline
  - build
  - test
  - docker
  - workflow
priority: high
cost: low
---

# GitHub Actions CI/CD

## Pipeline Template

```yaml
# .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

env:
  DOTNET_VERSION: '10.x'
  IMAGE_NAME: ${{ github.repository }}

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore --configuration Release

      - name: Test
        run: dotnet test --no-build --configuration Release --verbosity normal

  docker-build:
    needs: build-and-test
    runs-on: ubuntu-latest
    if: github.event_name == 'push' && github.ref == 'refs/heads/main'
    steps:
      - uses: actions/checkout@v4

      - name: Build Docker image
        run: docker build -t ${{ env.IMAGE_NAME }}:${{ github.sha }} .

      - name: Tag latest
        run: docker tag ${{ env.IMAGE_NAME }}:${{ github.sha }} ${{ env.IMAGE_NAME }}:latest
```

## NuGet Configuration in CI

All packages are from public nuget.org feed. No additional NuGet configuration needed.

If you need to add a private feed in the future, you can configure it with:

```yaml
      - name: Setup NuGet
        run: |
          dotnet nuget add source \
            "https://your-private-feed-url" \
            --name private-feed \
            --username ${{ github.actor }} \
            --password ${{ secrets.NUGET_PAT }} \
            --store-password-in-clear-text
```

## Deployment Pipeline (Optional)

```yaml
  deploy:
    needs: docker-build
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    environment: production
    steps:
      - name: Deploy
        run: |
          docker-compose pull
          docker-compose up -d
```

## Rules

- CI runs on every push and PR to `main`
- Build and test must pass before Docker build
- Docker build only runs on push to `main` (not on PRs)
- Never store secrets in workflow files — use GitHub Secrets
- Use `--no-restore` and `--no-build` flags to avoid redundant work
- Tag Docker images with both `sha` and `latest`

## Health Check After Deploy

```yaml
      - name: Health check
        run: |
          sleep 10
          curl --fail http://localhost:5000/health || exit 1
```

See also: `docker.md`, `git-workflow.md`.
