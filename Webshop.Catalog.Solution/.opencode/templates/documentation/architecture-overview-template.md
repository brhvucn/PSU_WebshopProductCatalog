# Architecture Overview: [Project Name]

*Synthesised from `.artifacts/architecture/` and `.artifacts/decisions/`*
*Last updated: [YYYY-MM-DD]*

---

## System Purpose

```txt
[What problem does this system solve? Who uses it?
Source: .artifacts/discovery/ and .artifacts/intake/]
```

---

## Architecture Style

3-layer architecture with service-oriented design.

**Architecture Type:** [Monolith / Microservices]

```
Api → Business → Data
```

| Layer | Responsibility |
|---|---|
| `Api` | HTTP boundary — controllers, middleware, composition root |
| `Application` | Use cases — commands, queries, handlers, validators |
| `Domain` | Business rules — entities, value objects, errors |
| `Infrastructure` | Data access — Dapper repositories, migrations |

---

## Key Architectural Decisions

*Source: `.artifacts/decisions/`*

| Decision | Rationale | ADR |
|---|---|---|
| [Decision] | [Why] | `docs/decisions/ADR-NNN.md` |

---

## Domain Model

*Source: `.artifacts/architecture/domain-model.md`*

| Entity | Purpose |
|---|---|
| [Entity] | [What it represents] |

---

## Integrations

| System | Type | Purpose |
|---|---|---|
| PostgreSQL | Database | Primary data store |
| [Other] | [REST/Event/Queue] | [Purpose] |

---

## Deployment

Docker / docker-compose. See `README.md` for run instructions.

---

## Open Questions

<!-- TODO: verify -->
- ...
