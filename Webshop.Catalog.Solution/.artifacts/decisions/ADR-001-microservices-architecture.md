# ADR-001: Microservices Architecture

**Status:** Accepted (existing decision)  
**Date:** 2026-09-27 (documented)  
**Context:** Brownfield project - decision already implemented

---

## Context

This is a teaching demo project for students learning microservices architecture. The project demonstrates how to build and deploy multiple independent services that work together to form a complete webshop system.

---

## Decision

**Architecture Style:** Microservices

The system is composed of multiple independent services:
- **Catalog API** - Product catalog management
- **Customer API** - Customer data management
- **Payment API** - Payment processing simulation
- **Review API** - Product review management
- **Help Service** - Database seeding and overview UI

---

## Rationale

### Why Microservices (for this demo)

1. **Educational Value**
   - Students learn service boundaries
   - Students learn inter-service communication
   - Students learn independent deployment
   - Students learn Docker containerization

2. **Clear Separation of Concerns**
   - Each service has single responsibility
   - Easy to understand individual services
   - Clear API boundaries

3. **Technology Demonstration**
   - Shows Docker Compose orchestration
   - Shows service discovery
   - Shows centralized logging (Seq)
   - Shows metrics collection (Prometheus)

4. **Realistic Architecture**
   - Mirrors real-world e-commerce systems
   - Prepares students for industry practices

---

## Consequences

### Positive

- ✅ Each service can be understood independently
- ✅ Clear demonstration of microservices patterns
- ✅ Students learn Docker and containerization
- ✅ Services can be developed/tested independently
- ✅ Realistic complexity for learning

### Negative

- ⚠️ More complex than monolith for simple demo
- ⚠️ Requires Docker knowledge
- ⚠️ More moving parts to debug
- ⚠️ Network communication overhead

### Mitigations

- All services run via single `docker-compose up` command
- Centralized logging in Seq for easy debugging
- Swagger UI on each service for easy testing
- Help service provides overview and database reset

---

## Alternatives Considered

### Monolith
- **Pros:** Simpler deployment, easier debugging
- **Cons:** Doesn't teach microservices concepts
- **Decision:** Rejected - project goal is to teach microservices

---

## Implementation Details

### Service Communication
- HTTP/REST between services
- Each service has own database schema (or separate database)
- No direct database sharing between services

### Deployment
- Docker Compose for local development
- Each service has own Dockerfile
- Shared SQL Server container for demo simplicity

### Infrastructure
- Seq for centralized logging
- Prometheus for metrics
- Smtp4Dev for email testing
- SQL Server for data persistence

---

## Related Decisions

- Technology stack: .NET 10, Controller-based APIs
- Database: MSSQL in Docker
- Deployment: Docker Compose only (no Kubernetes for simplicity)

---

## Notes

This is a **brownfield** decision - the architecture was already implemented. This ADR documents the existing choice for future reference and student understanding.
