---
domain: backend
capabilities:
  - architecture
keywords:
  - testing
  - strategy
  - unit test
  - integration
priority: high
cost: low
---

# Testing Strategy

## Backend

| Layer | Test type | Tool |
|---|---|---|
| Handlers | Unit test | xUnit + Moq |
| Domain logic | Unit test | xUnit |
| Full HTTP stack | Integration test | WebApplicationFactory |

## Frontend

| Layer | Test type | Tool |
|---|---|---|
| Vue components | Unit / component test | Vitest + Vue Test Utils |
| Pinia stores | Unit test | Vitest |
| Service layer | Unit test | Vitest + vi.mock(apiService) |

See `frontend/testing.md` for setup, examples and conventions.

## Backend Rules

- Unit tests mock repository interfaces.
- Unit tests should not use a real database.
- Domain tests should focus on business rules and invariants.
- Handler tests should verify orchestration and expected results.
- Integration tests should use the full application stack.
- Integration tests may use a real test database or test containers.

## Frontend Rules

- Mock apiService in all service tests — never call real endpoints.
- Test component rendering and user interaction, not implementation details.
- Test Pinia store actions and state transitions.
- Run: `npm test` (or `npx vitest run`).