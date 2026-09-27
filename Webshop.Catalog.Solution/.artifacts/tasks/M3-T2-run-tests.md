# Task: M3-T2 - Run All Tests Locally

**Milestone:** M3 - Build & Docker Verification  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 10 minutes

---

## Objective

Run all unit and integration tests to ensure no regressions from modernization

---

## Prerequisites

- M3-T1: Full solution build succeeds ✅

---

## Implementation Steps

1. Navigate to solution root

2. Run all tests:
   ```powershell
   dotnet test --no-build -c Release
   ```

3. Document results:
   - Total tests run
   - Passed
   - Failed
   - Skipped
   - Test execution time

---

## Test Projects

1. Webshop.Catalog.Application.Test
2. Webshop.Customer.Application.Test
3. Webshop.Payment.Test
4. Webshop.Review.Application.Test
5. Webshop.Review.API.Tests

---

## Verification

- [ ] All tests execute
- [ ] Zero test failures
- [ ] Document any skipped tests
- [ ] Test execution completes in reasonable time

---

## Expected Output

```
Test run for [projects]
Total tests: XX
     Passed: XX
     Failed: 0
    Skipped: 0
 Total time: X.XXXXs
```

---

## Troubleshooting

If tests fail:
1. Identify which test project failed
2. Read failure message
3. Determine if failure is related to recent changes
4. Check if test needs updating for .NET 10
5. Document issue

Common issues after modernization:
- DI container configuration differences
- Middleware order changes
- Configuration loading differences

---

## Files Changed

None (verification only)

---

## Dependencies

Depends on: M3-T1
