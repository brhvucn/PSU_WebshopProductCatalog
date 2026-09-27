# Intake: Brownfield Modernization for Student Demo

**Date:** 2026-09-27  
**Type:** Brownfield maintenance  
**Priority:** High  
**Context:** Teaching demo - must build and run via Docker Compose

---

## Raw Requirements

User's exact words:

> 1: Alt skal bruge .net 10. Nyligt opdateret
> 2: Dette projekt skal bruge mssql via docker som er inkluderet, det er en demo på microservices så det hele skal være contained i docker
> 3: Dette er et brownfield, jeg har kopieret opencode ind i eksisterende struktur
> 4: Skift dette, til moderne
> 5: harmoniser dette, så alle bruger samme
> 
> Næste skridt
> a: rapid, det er kun tilpasninger og mindre rettelser
> b: ignorer frontend, brug det som er, det er primært via api det her, "Help" solutionen kun er oversigt og seed/reset database
> c: Brug nuværende tilstand
> d: Brug nuværende, mssql via docker compose
> 
> Mit fokus er at få det hurtigt rettet, således det kan builde (se github action yml file) og det kan fungere som demo for mine studerende. De får således kun docker compose fil og så skal det kunne fungere

---

## Identified Issues

### 1. Legacy Startup Pattern
- **Services affected:** Catalog API, Customer API, Payment API
- **Issue:** Using old `Startup.cs` + `UseStartup<T>()` pattern
- **Target:** Modernize to .NET 10 minimal hosting model

### 2. Inconsistent Package Versions
- **Serilog versions:**
  - Payment API: 2.11.0
  - Customer API: 2.11.0
  - Catalog API: 3.1.1
  - Review API: 4.0.0
  - Help: 4.0.0

- **Swashbuckle versions:**
  - Payment API: 5.6.3
  - Customer API: 5.6.3
  - Catalog API: 6.6.1
  - Review API: 6.6.2
  - ReviewServiceGateway: 6.6.2

- **Prometheus:** All use 8.2.1 ✅ (consistent)

### 3. Build Requirements
- Must build via GitHub Actions (.NET 10 SDK)
- Must run via Docker Compose
- Students receive only docker-compose.yml

---

## Success Criteria

1. ✅ All projects build successfully with `dotnet build`
2. ✅ All tests pass with `dotnet test`
3. ✅ GitHub Actions workflow succeeds
4. ✅ All services use modern .NET 10 hosting model
5. ✅ All services use consistent package versions
6. ✅ Docker Compose starts all services successfully
7. ✅ Students can run demo with only `docker-compose up`

---

## Out of Scope

- Frontend changes (use existing Help service UI)
- Database migration from MSSQL
- Architecture changes
- New features

---

## Constraints

- **Time:** Quick fixes only (rapid track)
- **Audience:** Students learning microservices
- **Deployment:** Docker Compose only
- **Database:** MSSQL in Docker container
