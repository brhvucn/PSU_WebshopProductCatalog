# Task: M1-T4 - Update Swashbuckle in Customer API

**Milestone:** M1 - Package Harmonization  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 5 minutes

---

## Objective

Update Swashbuckle.AspNetCore in Customer API to version 6.6.2

---

## Current State

Customer API uses: Swashbuckle.AspNetCore 5.6.3

---

## Target State

Customer API uses: Swashbuckle.AspNetCore 6.6.2 (matching Review API)

---

## Implementation Steps

1. Open `Webshop.Customer.Api\Webshop.Customer.Api.csproj`
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

- `Webshop.Customer.Api\Webshop.Customer.Api.csproj`

---

## Dependencies

None
