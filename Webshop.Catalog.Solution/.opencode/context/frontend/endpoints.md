---
domain: frontend
capabilities:
  - endpoints
  - api-routes
keywords:
  - endpoints
  - api routes
  - url management
priority: medium
cost: low
---

# Endpoints

All endpoint strings are collected in `src/services/endpoints.js`.

## Principle

One file for all API paths. Services reference endpoints by key, never by hardcoded strings.

## Structure

```js
const API = `${BASE}/api`

export default {
  BASEURL: API,

  RESOURCE: {
    GETALL: () => `${API}/resource`,
    GETBYID: (id) => `${API}/resource/${id}`,
    CREATE: () => `${API}/resource`,
    UPDATE: (id) => `${API}/resource/${id}`,
    DELETE: (id) => `${API}/resource/${id}`,
  },
}
```

## Rules

- Group endpoints by domain (`AUTH`, `USERS`, `ORGANISATIONS`).
- Use arrow functions returning strings for dynamic segments.
- Services call `endpoints.DOMAIN.METHOD()`, never construct URLs manually.

See `.opencode/context/examples/frontend-endpoints.js.md` for the full implementation.
