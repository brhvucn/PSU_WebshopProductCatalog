# Project Milestones

**Project:** Webshop Microservices Demo - Brownfield Modernization  
**Track:** Rapid  
**Created:** 2026-09-27

---

## Milestone 1: Package Harmonization ⏳

**Goal:** Standardize all NuGet package versions across microservices

**Why:** Inconsistent versions cause build issues and confusion for students

**Deliverables:**
- All services use same Serilog version (4.0.0)
- All services use same Swashbuckle version (6.6.2)
- All services use same Prometheus version (8.2.1) ✅ already consistent

**Tasks:**
- [ ] M1-T1: Update Serilog packages to v4.0.0 in Payment API
- [ ] M1-T2: Update Serilog packages to v4.0.0 in Customer API
- [ ] M1-T3: Update Swashbuckle to v6.6.2 in Payment API
- [ ] M1-T4: Update Swashbuckle to v6.6.2 in Customer API
- [ ] M1-T5: Verify all projects build after package updates

**Acceptance:**
- `dotnet build` succeeds for entire solution
- No package version conflicts

---

## Milestone 2: Modernize to .NET 10 Hosting Model ⏳

**Goal:** Replace legacy Startup.cs pattern with modern minimal hosting

**Why:** .NET 10 best practices, simpler code, better student learning experience

**Deliverables:**
- Catalog API uses minimal hosting
- Customer API uses minimal hosting
- Payment API uses minimal hosting
- All Startup.cs files removed

**Tasks:**
- [ ] M2-T1: Modernize Catalog API (Program.cs + remove Startup.cs)
- [ ] M2-T2: Modernize Customer API (Program.cs + remove Startup.cs)
- [ ] M2-T3: Modernize Payment API (Program.cs + remove Startup.cs)
- [ ] M2-T4: Verify all APIs start and respond to health checks

**Acceptance:**
- All APIs use WebApplication.CreateBuilder() pattern
- No Startup.cs files remain
- Swagger UI accessible on all APIs
- Health endpoints respond

---

## Milestone 3: Build & Docker Verification ⏳

**Goal:** Ensure GitHub Actions build passes and Docker Compose works

**Why:** Students must be able to run demo with single command

**Deliverables:**
- GitHub Actions workflow passes
- Docker Compose starts all services
- All services healthy and accessible

**Tasks:**
- [ ] M3-T1: Run full solution build locally
- [ ] M3-T2: Run all tests locally
- [ ] M3-T3: Test Docker Compose startup
- [ ] M3-T4: Verify service connectivity (API → Database)
- [ ] M3-T5: Document any Docker Compose usage notes

**Acceptance:**
- `dotnet build` succeeds
- `dotnet test` passes
- `docker-compose up` starts all services
- All health checks green
- Swagger accessible on all API ports

---

## Status Summary

| Milestone | Status | Tasks | Blocked |
|-----------|--------|-------|---------|
| M1: Package Harmonization | ⏳ Pending | 0/5 | No |
| M2: Modernize Hosting | ⏳ Pending | 0/4 | No |
| M3: Build & Docker | ⏳ Pending | 0/5 | No |

**Total:** 0/14 tasks completed
