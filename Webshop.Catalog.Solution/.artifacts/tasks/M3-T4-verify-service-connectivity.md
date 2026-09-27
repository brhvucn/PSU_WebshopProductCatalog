# Task: M3-T4 - Verify Service Connectivity

**Milestone:** M3 - Build & Docker Verification  
**Status:** [ ] Pending  
**Priority:** High  
**Estimated effort:** 15 minutes

---

## Objective

Verify that all services can communicate with each other and with the database

---

## Prerequisites

- M3-T3: Docker Compose running ✅

---

## Implementation Steps

### 1. Database Connectivity

For each API, verify database connection:

**Catalog API:**
- Check logs for successful database connection
- Test a GET endpoint that queries database
- Verify data returns correctly

**Customer API:**
- Check logs for successful database connection
- Test CRUD operations
- Verify database operations work

**Review API:**
- Check logs for successful database connection
- Test review endpoints
- Verify database operations work

### 2. Logging to Seq

- Open Seq UI: http://localhost:8081
- Verify logs appearing from all services:
  - Catalog API
  - Customer API
  - Payment API
  - Review API
  - Help Service

### 3. Metrics to Prometheus

- Open Prometheus UI: http://localhost:8087
- Verify metrics endpoints discovered
- Check that metrics are being scraped

### 4. API Functionality

Test basic operations on each API:

**Payment API:**
- Test payment simulation endpoint
- Verify business logic works

**Catalog API:**
- GET /api/products (or similar)
- GET /api/categories (or similar)
- Verify data returns

**Customer API:**
- GET /api/customers (or similar)
- POST /api/customers (create test customer)
- Verify CRUD works

**Review API:**
- Test review endpoints
- Verify functionality

---

## Verification Checklist

### Database Connectivity
- [ ] Catalog API connects to database
- [ ] Customer API connects to database
- [ ] Review API connects to database
- [ ] No connection errors in logs

### Logging
- [ ] Seq receives logs from Catalog API
- [ ] Seq receives logs from Customer API
- [ ] Seq receives logs from Payment API
- [ ] Seq receives logs from Review API
- [ ] Seq receives logs from Help Service

### Metrics
- [ ] Prometheus scrapes Catalog API metrics
- [ ] Prometheus scrapes Customer API metrics
- [ ] Prometheus scrapes Payment API metrics
- [ ] Prometheus scrapes Review API metrics

### API Functionality
- [ ] Payment API endpoints respond
- [ ] Catalog API endpoints respond
- [ ] Customer API endpoints respond
- [ ] Review API endpoints respond
- [ ] All responses are valid JSON

---

## Test Endpoints

Use Swagger UI or curl/Postman to test:

```powershell
# Catalog API
curl http://localhost:8084/api/products

# Customer API
curl http://localhost:8085/api/customers

# Payment API
curl http://localhost:8083/api/payment

# Review API
curl http://localhost:8086/api/reviews
```

*(Adjust endpoints based on actual API routes)*

---

## Expected Results

1. All APIs respond to requests
2. Database queries return data
3. Logs appear in Seq
4. Metrics appear in Prometheus
5. No errors in container logs

---

## Troubleshooting

### No logs in Seq
- Check Serilog configuration in each API
- Verify Seq URL in appsettings.json or environment variables
- Check Seq container logs

### Database connection fails
- Verify connection string
- Check SQL Server container is healthy
- Verify network connectivity between containers

### Metrics not appearing
- Check Prometheus configuration (prometheus.yml)
- Verify metrics endpoints exposed on APIs
- Check Prometheus targets page

---

## Files Changed

None (verification only)

---

## Dependencies

Depends on: M3-T3 (Docker Compose running)
