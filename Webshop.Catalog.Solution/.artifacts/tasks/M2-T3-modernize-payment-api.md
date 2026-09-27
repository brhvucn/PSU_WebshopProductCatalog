# Task: M2-T3 - Modernize Payment API to .NET 10 Hosting Model

**Milestone:** M2 - Modernize to .NET 10 Hosting Model  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 20 minutes

---

## Objective

Replace legacy Startup.cs pattern with modern .NET 10 minimal hosting in Payment API

---

## Current State

Payment API uses:
- `Program.cs` with `CreateHostBuilder()` and `UseStartup<Startup>()`
- Separate `Startup.cs` with `ConfigureServices()` and `Configure()`

---

## Target State

Payment API uses:
- Modern `WebApplication.CreateBuilder()` pattern
- All configuration in `Program.cs`
- No `Startup.cs` file

---

## Implementation Steps

1. Read current `Startup.cs` to understand configuration
2. Rewrite `Program.cs` using modern pattern
3. Delete `Startup.cs`
4. Test build and functionality

---

## Verification

- [ ] Project builds without errors
- [ ] No Startup.cs file remains
- [ ] Swagger UI accessible
- [ ] Health endpoint responds
- [ ] Payment simulation logic intact

---

## Files Changed

- `Webshop.Payment.Api\Program.cs` (rewritten)
- `Webshop.Payment.Api\Startup.cs` (deleted)

---

## Dependencies

Can reference M2-T1 and M2-T2 as templates
