# Task: M2-T4 - Verify All APIs Start and Respond

**Milestone:** M2 - Modernize to .NET 10 Hosting Model  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 15 minutes

---

## Objective

Verify that all modernized APIs start successfully and respond to health checks

---

## Prerequisites

- M2-T1: Catalog API modernized ✅
- M2-T2: Customer API modernized ✅
- M2-T3: Payment API modernized ✅

---

## APIs to Test

1. **Catalog API** (port 8084 in Docker)
2. **Customer API** (port 8085 in Docker)
3. **Payment API** (port 8083 in Docker)
4. **Review API** (port 8086 in Docker) - already modern
5. **Help Service** (port 8000 in Docker)

---

## Implementation Steps

### Local Testing (if possible)

For each API:
1. Navigate to API directory
2. Run `dotnet run`
3. Check console output for errors
4. Access Swagger UI (usually at `/swagger`)
5. Test health endpoint (if configured)
6. Stop service (Ctrl+C)

### Build Verification

1. Navigate to solution root
2. Run `dotnet build --no-restore -c Release`
3. Verify all projects build successfully

---

## Verification Checklist

### Catalog API
- [ ] Builds without errors
- [ ] Starts without exceptions
- [ ] Swagger UI accessible
- [ ] Health endpoint responds (if configured)

### Customer API
- [ ] Builds without errors
- [ ] Starts without exceptions
- [ ] Swagger UI accessible
- [ ] Health endpoint responds (if configured)

### Payment API
- [ ] Builds without errors
- [ ] Starts without exceptions
- [ ] Swagger UI accessible
- [ ] Health endpoint responds (if configured)

### Review API
- [ ] Still builds (no regression)
- [ ] Starts without exceptions

### Help Service
- [ ] Still builds (no regression)
- [ ] Starts without exceptions

---

## Expected Endpoints

| Service | Swagger | Health | Port (Docker) |
|---------|---------|--------|---------------|
| Catalog | `/swagger` | `/health` | 8084 |
| Customer | `/swagger` | `/health` | 8085 |
| Payment | `/swagger` | `/health` | 8083 |
| Review | `/swagger` | `/health` | 8086 |
| Help | `/` | - | 8000 |

---

## Troubleshooting

If an API fails to start:
1. Check console error messages
2. Verify all dependencies registered in DI
3. Check middleware order in pipeline
4. Compare with working API (Review API)
5. Check for missing configuration

---

## Files Changed

None (verification only)

---

## Dependencies

Depends on: M2-T1, M2-T2, M2-T3
