---
purpose: Define system architecture, boundaries, and integration patterns
owner: architect
supports: [consultant, reviewer, developer]
phase: design
artifacts:
  produce:
    - .artifacts/architecture/
    - .artifacts/risks/
    - .artifacts/decisions/
  read:
    - .artifacts/discovery/
    - PROJECT-CONFIG.md
entryFrom: [discovery-review]
exitTo: [architecture-review]
commands: [/architect, /create-risk, /create-decision]
avoid: [planning, implementation, deployment]
---

# Architecture Workflow

Define system architecture for the solution using standardized 3-layer pattern (Api → Business → Data) with service-oriented design.

Focus: system boundaries, service responsibilities, integration patterns, communication, deployment topology.

---

## Architectural Baseline

**Default architecture (always):**
- 3-layer (Api → Business → Data)
- Service-oriented design
- Repository pattern
- Standard .NET validation
- Clear separation of concerns

**Project-specific decisions:**
- Monolith vs microservices (from intake)
- Tech stack (from PROJECT-CONFIG.md)
- Integration patterns
- Deployment topology

**Read PROJECT-CONFIG.md for:**
- Frontend: Nuxt / Vue / none
- Backend: Minimal API / Controller API
- Database: PostgreSQL / SQLite

Architecture aligns with tech choices.

---

## Sequence

```
/architect → define system boundaries → identify integration points →
document communication patterns → /create-risk (optional) →
/create-decision (optional) → architecture-review
```

---

## Architecture Concerns

**System Structure:**
- Monolith vs microservices (from intake decision)
- Service boundaries (if microservices)
- Module boundaries (if monolith)
- Shared kernel (if any)

**Service Responsibilities:**
- What each service/module owns
- Data ownership
- Business logic ownership
- Clear boundaries (no overlap)

**Integration Architecture:**
- How services/modules communicate
- Synchronous (HTTP, gRPC) or asynchronous (messaging)
- API contracts
- Event contracts (if event-driven)

**Communication Patterns:**
- Request/response
- Event-driven
- Choreography vs orchestration
- Error handling across boundaries

**API Boundaries:**
- Public APIs (external clients)
- Internal APIs (service-to-service)
- API versioning strategy
- Authentication/authorization boundaries

**Data Architecture:**
- Database per service (microservices)
- Shared database (monolith)
- Data consistency strategy
- Migration strategy (if applicable)

**Deployment Topology:**
- Containerization (Docker)
- Orchestration (Kubernetes, Docker Compose, none)
- Cloud vs on-premise
- Scaling strategy

---

## Monolith Architecture Pattern

**When chosen (from intake):**

**Structure:**
```
Solution/
├── Api/           — Controllers/endpoints, HTTP boundary
├── Business/      — Services, business logic
├── Data/          — Repositories, database access
└── Shared/        — DTOs, models, contracts
```

**Boundaries:**
- Logical modules within each layer
- Clear service responsibilities
- No circular dependencies

**Communication:**
- Direct method calls (in-process)
- Services injected via DI

**Data:**
- Single database
- Single schema (or schema per module)

**Deployment:**
- Single deployable unit
- Scale entire application

---

## Microservices Architecture Pattern

**When chosen (from intake):**

**Structure:**
```
Services/
├── UserService/
│   ├── Api/
│   ├── Business/
│   └── Data/
├── OrderService/
│   ├── Api/
│   ├── Business/
│   └── Data/
└── Gateway/       — API gateway (optional)
```

**Boundaries:**
- Service per bounded context
- Independent deployment
- Database per service

**Communication:**
- HTTP/REST or gRPC (sync)
- Message bus (async) if needed
- API contracts versioned

**Data:**
- Database per service
- Eventual consistency via events
- No direct database access across services

**Deployment:**
- Independent deployable units
- Scale services independently

---

## Integration Patterns

**Synchronous:**
- HTTP/REST (standard)
- gRPC (high performance)
- Request/response pattern

**Asynchronous:**
- Message queue (RabbitMQ, Azure Service Bus)
- Event bus (publish/subscribe)
- Event-driven architecture

**Hybrid:**
- Commands via HTTP
- Events via message bus
- CQRS if needed

**Recommendation:**
- Start with HTTP/REST (simplest)
- Add async when decoupling needed
- Event-driven when eventual consistency acceptable

---

## Technology Alignment

**Read PROJECT-CONFIG.md to align architecture with tech choices:**

**Minimal API:**
- Endpoint handlers (static methods)
- Endpoint grouping (MapGroup)
- Endpoint filters (validation, auth)

**Controller API:**
- MVC controllers
- Action methods
- Attribute routing

**PostgreSQL:**
- Production database
- Migrations via EF or manual scripts
- Connection pooling

**SQLite:**
- Prototype database
- File-based storage
- Simple setup (no Docker)

**Vue/Nuxt:**
- SPA or SSR frontend
- API client via axios
- State via Pinia

---

## Architecture Artifacts

**Required:**
- `architecture.md` — system overview, boundaries, patterns
- `domain-model.md` — entities, aggregates, relationships
- ADRs for significant decisions

**Optional:**
- `integration-architecture.md` — service communication
- C4 context diagram
- Deployment diagram

Use templates from `.opencode/templates/`.

---

## Risk Identification

**Common architectural risks:**
- Tight coupling between services/modules
- Unclear service boundaries
- Data consistency across boundaries
- Performance bottlenecks
- Scalability limits
- Security boundaries unclear
- Deployment complexity

**Action:**
Create `.artifacts/risks/YYYY-MM-DD-risk-name.md` for each identified risk.

---

## Architecture Decision Records (ADRs)

**When to create ADR:**
- Significant architectural choice (monolith vs microservices)
- Integration pattern choice (sync vs async)
- Technology choice with long-term impact
- Data architecture decision
- Security boundary decision

**Format:**
```markdown
# ADR-NNN: Title

**Status:** Proposed | Accepted | Deprecated
**Date:** YYYY-MM-DD
**Context:** What is the issue?
**Decision:** What are we doing?
**Consequences:** What are the tradeoffs?
```

Template: `.opencode/templates/adr-template.md`

---

## Workflow Boundaries

**Entry:**
- Discovery review approved
- Business value clear
- Stakeholders identified
- Constraints understood

**Exit:**
- Architecture defined
- Boundaries clear
- Integration patterns decided
- Risks identified
- Architecture review gate passed

**May transition to:**
- Architecture review (normal flow)
- Discovery (if context insufficient)

**Must not:**
- Over-engineer (keep simple)
- Skip review gate
- Commit to implementation detail
- Ignore PROJECT-CONFIG tech choices

---

## Architecture Principles

**Keep simple:**
- Start with monolith unless microservices required
- Use standard patterns
- Avoid premature optimization

**Align with tech stack:**
- Read PROJECT-CONFIG.md
- Use Minimal API or Controller API as configured
- PostgreSQL or SQLite as configured

**Support incremental delivery:**
- Architecture emerges through implementation
- Allow refinement
- Document significant changes as ADRs

**Clear boundaries:**
- Service/module responsibilities explicit
- No overlapping ownership
- Integration points defined

**Maintainability over cleverness:**
- Standard patterns
- Clear separation
- Testable design

---

## Operating Principle

```
Preserve simplicity. Support incremental architecture. Maintain clear boundaries.
Align with tech choices. Document significant decisions.
```
