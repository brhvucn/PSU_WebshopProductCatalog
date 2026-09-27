# Task: M1-T3 - Update Swashbuckle in Payment API

**Milestone:** M1 - Package Harmonization  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 5 minutes

---

## Objective

Update Swashbuckle.AspNetCore in Payment API to version 6.6.2

---

## Current State

Payment API uses: Swashbuckle.AspNetCore 5.6.3

---

## Target State

Payment API uses: Swashbuckle.AspNetCore 6.6.2 (matching Review API)

---

## Implementation Steps

1. Open `Webshop.Payment.Api\Webshop.Payment.Api.csproj`
2. Update Swashbuckle.AspNetCore version to 6.6.2
3. Save file
4. Run `dotnet restore`
5. Run `dotnet build` to verify

---

## Verification

- [ ] Project builds without errors
- [ ] Swagger UI still accessible
- [ ] Version matches Review API

---

## Files Changed

- `Webshop.Payment.Api\Webshop.Payment.Api.csproj`

---

## Dependencies

None
