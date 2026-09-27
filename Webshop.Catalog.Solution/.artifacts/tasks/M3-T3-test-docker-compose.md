# Task: M3-T3 - Test Docker Compose Startup

**Milestone:** M3 - Build & Docker Verification  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 20 minutes

---

## Objective

Verify that all services start successfully via Docker Compose (student experience)

---

## Prerequisites

- M3-T1: Solution builds ✅
- M3-T2: Tests pass ✅
- Docker Desktop running

---

## Implementation Steps

1. Navigate to solution root:
   ```powershell
   cd C:\Users\brhv\source\repos\brhvucn\PSU_WebshopProductCatalog\Webshop.Catalog.Solution
   ```

2. Stop any running containers:
   ```powershell
   docker-compose down
   ```

3. Remove old images (optional, for clean test):
   ```powershell
   docker-compose down --rmi local
   ```

4. Start all services:
   ```powershell
   docker-compose up --build
   ```

5. Monitor startup logs for errors

6. Wait for all services to be healthy

7. Test each service endpoint

---

## Services to Verify

| Service | Container Name | Port | Endpoint to Test |
|---------|---------------|------|------------------|
| SQL Server | webshopdatabase | 1403 | Connection test |
| Seq | SeQ | 8081 | http://localhost:8081 |
| Smtp4Dev | smtp4dev | 8082 | http://localhost:8082 |
| Payment API | webshop.payment | 8083 | http://localhost:8083/swagger |
| Catalog API | webshop.catalog | 8084 | http://localhost:8084/swagger |
| Customer API | webshop.customer | 8085 | http://localhost:8085/swagger |
| Review API | webshop.review | 8086 | http://localhost:8086/swagger |
| Help Service | webshophelp | 8000 | http://localhost:8000 |
| Prometheus | prometheus | 8087 | http://localhost:8087 |

---

## Verification Checklist

### Infrastructure Services
- [ ] SQL Server container starts
- [ ] SQL Server health check passes
- [ ] Seq UI accessible
- [ ] Smtp4Dev UI accessible
- [ ] Prometheus UI accessible

### API Services
- [ ] Payment API container starts
- [ ] Payment API Swagger accessible
- [ ] Catalog API container starts
- [ ] Catalog API Swagger accessible
- [ ] Customer API container starts
- [ ] Customer API Swagger accessible
- [ ] Review API container starts
- [ ] Review API Swagger accessible
- [ ] Help Service container starts
- [ ] Help Service UI accessible

### Database Connectivity
- [ ] APIs can connect to SQL Server
- [ ] No connection errors in logs
- [ ] Database tables created (if using migrations)

---

## Expected Behavior

1. All containers start without errors
2. SQL Server health check passes before APIs start
3. All API Swagger UIs accessible
4. No connection errors in logs
5. Seq shows logs from all services

---

## Common Issues

### Issue: API can't connect to database
- **Cause:** SQL Server not ready yet
- **Solution:** Check depends_on and health check in docker-compose.yml

### Issue: Port already in use
- **Cause:** Previous containers still running
- **Solution:** Run `docker-compose down` first

### Issue: Image build fails
- **Cause:** Dockerfile issues or build errors
- **Solution:** Check Dockerfile, verify .NET 10 SDK available

---

## Cleanup

After testing:
```powershell
docker-compose down
```

To remove volumes (reset database):
```powershell
docker-compose down -v
```

---

## Files Changed

None (verification only)

---

## Dependencies

Depends on: M3-T1, M3-T2

---

## Notes

This simulates the exact student experience - they should be able to run `docker-compose up` and have everything work.
