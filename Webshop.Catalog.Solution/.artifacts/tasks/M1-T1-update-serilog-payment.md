# Task: M1-T1 - Update Serilog Packages in Payment API

**Milestone:** M1 - Package Harmonization  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 5 minutes

---

## Objective

Update Serilog and related packages in Payment API to version 4.0.0 (harmonize with Review API and Help service)

---

## Current State

Payment API uses outdated Serilog versions:
- Serilog: 2.11.0
- Serilog.Extensions.Logging: 3.1.0
- Serilog.Sinks.Console: 4.0.1
- Serilog.Sinks.Seq: 5.1.1

---

## Target State

Payment API uses current Serilog versions:
- Serilog: 4.0.0
- Serilog.Extensions.Logging: 8.0.0
- Serilog.Sinks.Console: 5.0.1 (or latest compatible)
- Serilog.Sinks.Seq: 8.0.0

---

## Implementation Steps

1. Open `Webshop.Payment.Api\Webshop.Payment.Api.csproj`
2. Update Serilog package versions to match Review API
3. Save file
4. Run `dotnet restore` in Payment API directory
5. Run `dotnet build` to verify

---

## Verification

- [ ] Project builds without errors
- [ ] No package conflict warnings
- [ ] Serilog versions match Review API

---

## Files Changed

- `Webshop.Payment.Api\Webshop.Payment.Api.csproj`

---

## Dependencies

None - can be done independently

---

## Notes

Review API already uses Serilog 4.0.0 successfully, so this is a safe upgrade.
