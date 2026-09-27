# Task: M3-T5 - Document Docker Compose Usage Notes

**Milestone:** M3 - Build & Docker Verification  
**Status:** [ ] Pending  
**Priority:** Medium  
**Estimated effort:** 15 minutes

---

## Objective

Create simple documentation for students on how to run the demo

---

## Prerequisites

- M3-T3: Docker Compose verified ✅
- M3-T4: Service connectivity verified ✅

---

## Implementation Steps

1. Create or update README.md in solution root
2. Document:
   - Prerequisites (Docker Desktop)
   - How to start services
   - How to access each service
   - How to stop services
   - How to reset database
   - Troubleshooting common issues

3. Keep it simple and student-friendly

---

## Content to Include

### Prerequisites
- Docker Desktop installed and running
- Ports 8000-8087 and 1403 available

### Quick Start
```powershell
# Start all services
docker-compose up

# Or run in background
docker-compose up -d
```

### Access Services
- Catalog API: http://localhost:8084/swagger
- Customer API: http://localhost:8085/swagger
- Payment API: http://localhost:8083/swagger
- Review API: http://localhost:8086/swagger
- Help Service: http://localhost:8000
- Seq (Logs): http://localhost:8081
- Smtp4Dev (Email): http://localhost:8082
- Prometheus (Metrics): http://localhost:8087

### Stop Services
```powershell
docker-compose down
```

### Reset Database
```powershell
docker-compose down -v
docker-compose up
```

### Troubleshooting
- Port conflicts: Stop other services using ports 8000-8087
- Database not ready: Wait for SQL Server health check to pass
- Build errors: Ensure .NET 10 SDK installed

---

## Verification

- [ ] README.md created or updated
- [ ] All commands tested and verified
- [ ] URLs tested and correct
- [ ] Student-friendly language used
- [ ] Common issues documented

---

## Files Changed

- `README.md` (created or updated)

---

## Dependencies

Depends on: M3-T3, M3-T4

---

## Notes

Keep documentation minimal - students should be able to run `docker-compose up` and explore via Swagger.
