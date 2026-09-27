# Task: M2-T1 - Modernize Catalog API to .NET 10 Hosting Model

**Milestone:** M2 - Modernize to .NET 10 Hosting Model  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 20 minutes

---

## Objective

Replace legacy Startup.cs pattern with modern .NET 10 minimal hosting in Catalog API

---

## Current State

Catalog API uses:
- `Program.cs` with `CreateHostBuilder()` and `UseStartup<Startup>()`
- Separate `Startup.cs` with `ConfigureServices()` and `Configure()`

---

## Target State

Catalog API uses:
- Modern `WebApplication.CreateBuilder()` pattern
- All configuration in `Program.cs`
- No `Startup.cs` file

---

## Implementation Steps

1. Read current `Startup.cs` to understand:
   - Service registrations (DI)
   - Middleware pipeline
   - Serilog configuration
   - Swagger setup
   - Prometheus metrics
   - Health checks

2. Rewrite `Program.cs`:
   - Use `WebApplication.CreateBuilder(args)`
   - Move service registrations to builder.Services
   - Move middleware to app pipeline
   - Preserve all existing functionality

3. Delete `Startup.cs`

4. Test:
   - Build project
   - Run locally (if possible)
   - Verify Swagger UI accessible
   - Verify health endpoint

---

## Key Patterns to Preserve

- Serilog logging to Seq
- Swagger/OpenAPI documentation
- Prometheus metrics endpoint
- Health checks
- CORS configuration (if any)
- Database connection setup

---

## Verification

- [ ] Project builds without errors
- [ ] No Startup.cs file remains
- [ ] Swagger UI accessible at `/swagger`
- [ ] Health endpoint responds
- [ ] Serilog configuration intact
- [ ] Prometheus metrics endpoint works

---

## Files Changed

- `Webshop.Catalog.Api\Program.cs` (rewritten)
- `Webshop.Catalog.Api\Startup.cs` (deleted)

---

## Dependencies

None - can be done after M1 completes

---

## Reference

Review API likely already uses modern pattern - check for reference implementation
