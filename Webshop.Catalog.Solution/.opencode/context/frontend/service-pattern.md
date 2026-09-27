---
domain: frontend
capabilities:
  - service-layer
keywords:
  - service
  - api layer
  - data access
priority: medium
cost: low
---

# Service Layer

Services encapsulate API calls for a specific domain. Components and views call services, never Axios directly.

## Pattern

```js
import apiService from './apiService'
import endpoints from './endpoints'

export default {
  async loadAll() {
    const result = await apiService.get(endpoints.DOMAIN.GETALL())
    return result.result
  },

  async getById(id) {
    const result = await apiService.get(endpoints.DOMAIN.GETBYID(id))
    return result.result
  },

  async create(data) {
    const result = await apiService.post(endpoints.DOMAIN.CREATE(), data)
    return result.result
  },
}
```

## Rules

- One service file per domain.
- Services return extracted data (`result.result`), not raw Axios responses.
- Services must not know about components, routes or UI state.

See example: `.opencode/context/examples/frontend-usersService.js.md`
