# Execution Plan: Brownfield Modernization

**Project:** Webshop Microservices Demo  
**Track:** Rapid  
**Goal:** Get demo building and running for students  
**Created:** 2026-09-27

---

## Overview

This is a brownfield .NET microservices project that needs:
1. Package version harmonization
2. Modernization to .NET 10 hosting model
3. Build and Docker verification

**Target:** Students can run `docker-compose up` and have a working microservices demo.

---

## Milestones

### ✅ M0: Project Configuration (COMPLETE)
- [x] Configure PROJECT-CONFIG.md
- [x] Set track: rapid
- [x] Set backend: controller-api
- [x] Set database: mssql
- [x] Document architecture decision (ADR-001)
- [x] Update techstack overview

### ⏳ M1: Package Harmonization
**Goal:** All services use consistent package versions

**Tasks:**
1. M1-T1: Update Serilog in Payment API (2.11.0 → 4.0.0)
2. M1-T2: Update Serilog in Customer API (2.11.0 → 4.0.0)
3. M1-T3: Update Swashbuckle in Payment API (5.6.3 → 6.6.2)
4. M1-T4: Update Swashbuckle in Customer API (5.6.3 → 6.6.2)
5. M1-T5: Verify solution builds

**Estimated time:** 30 minutes

### ⏳ M2: Modernize to .NET 10 Hosting
**Goal:** Replace legacy Startup.cs with modern Program.cs

**Tasks:**
1. M2-T1: Modernize Catalog API
2. M2-T2: Modernize Customer API
3. M2-T3: Modernize Payment API
4. M2-T4: Verify all APIs start and respond

**Estimated time:** 1 hour

### ⏳ M3: Build & Docker Verification
**Goal:** Ensure GitHub Actions passes and Docker Compose works

**Tasks:**
1. M3-T1: Run full solution build
2. M3-T2: Run all tests
3. M3-T3: Test Docker Compose startup
4. M3-T4: Verify service connectivity
5. M3-T5: Document usage for students

**Estimated time:** 1 hour

---

## Total Effort Estimate

**Total:** ~2.5 hours of focused work

---

## Success Criteria

- [x] PROJECT-CONFIG.md configured
- [ ] All services use Serilog 4.0.0
- [ ] All services use Swashbuckle 6.6.2
- [ ] All services use modern .NET 10 hosting
- [ ] No Startup.cs files remain
- [ ] `dotnet build` succeeds
- [ ] `dotnet test` passes
- [ ] GitHub Actions workflow passes
- [ ] `docker-compose up` starts all services
- [ ] All Swagger UIs accessible
- [ ] All services connect to database
- [ ] Logs appear in Seq
- [ ] Metrics appear in Prometheus
- [ ] Student documentation complete

---

## Risk Assessment

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| Package upgrade breaks code | Low | Medium | Review API already uses newer versions successfully |
| Modernization breaks DI | Low | High | Can reference Review API as working example |
| Docker build fails | Low | High | Test locally first, verify Dockerfiles |
| Database connection issues | Medium | Medium | Connection strings already configured |
| Tests fail after changes | Low | Medium | Minimal code changes, mostly configuration |

---

## Rollback Plan

If critical issues arise:
1. Git is initialized - can revert commits
2. Each milestone is independent - can rollback specific changes
3. Package updates can be reverted individually
4. Startup.cs files can be restored if modernization fails

---

## Next Steps

**For Developer (@ucn-developer):**

1. Start with M1-T1 (Update Serilog in Payment API)
2. Work through M1 tasks sequentially
3. Verify build after M1-T5
4. Proceed to M2 tasks
5. Complete with M3 verification tasks

**For Consultant (me):**

- Monitor progress
- Review completed milestones
- Adjust plan if issues discovered
- Create additional tasks if needed

---

## Notes

- This is a **brownfield** project - existing code is working
- Changes are **low-risk** - mostly package updates and hosting model
- **Rapid track** - minimal governance, focus on getting it working
- **Teaching demo** - students need simple `docker-compose up` experience
- All services already target .NET 10 - just need modernization
