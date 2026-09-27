# Task: M1-T2 - Update Serilog Packages in Customer API

**Milestone:** M1 - Package Harmonization  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 5 minutes

---

## Objective

Update Serilog and related packages in Customer API to version 4.0.0

---

## Current State

Customer API uses outdated Serilog versions:
- Serilog: 2.11.0
- Serilog.Extensions.Logging: 3.1.0
- Serilog.Sinks.Console: 4.0.1
- Serilog.Sinks.Seq: 5.1.1

---

## Target State

Customer API uses current Serilog versions:
- Serilog: 4.0.0
- Serilog.Extensions.Logging: 8.0.0
- Serilog.Sinks.Console: 5.0.1
- Serilog.Sinks.Seq: 8.0.0

---

## Implementation Steps

1. Open `Webshop.Customer.Api\Webshop.Customer.Api.csproj`
2. Update Serilog package versions to match Review API
3. Save file
4. Run `dotnet restore` in Customer API directory
5. Run `dotnet build` to verify

---

## Verification

- [ ] Project builds without errors
- [ ] No package conflict warnings
- [ ] Serilog versions match Review API

---

## Files Changed

- `Webshop.Customer.Api\Webshop.Customer.Api.csproj`

---

## Dependencies

None - can be done independently
