---
domain: frontend
capabilities:
  - api-client
  - http
keywords:
  - axios
  - apiService
  - http client
  - interceptor
  - token refresh
priority: high
cost: low
---

# API Client

All HTTP calls go through a shared Axios instance (`apiService.js`).

## Responsibilities

- Centralize base URL and default headers
- Attach Bearer token from localStorage
- Handle 401 responses with automatic token refresh
- Expose methods: `get`, `post`, `put`, `del`, `getBlob`, `uploadFile`

## Token Refresh Flow

1. On 401, check if the failed request is already a refresh call → logout.
2. If another refresh is in progress, wait for it (shared promise pattern).
3. Otherwise, call `/auth/refresh`, store new tokens, retry original request.
4. If refresh fails → logout and redirect to `/login`.

## Key Pattern

```js
// apiService.get / post / put / del return response.data
const result = await apiService.get(endpoints.USERS.GETALL())
```

## Usage in Services

```js
import apiService from './apiService'
import endpoints from './endpoints'

export default {
  async loadAll() {
    return await apiService.get(endpoints.RESOURCE.GETALL())
  },
}
```

See `.opencode/context/examples/frontend-apiService.js.md` for the full implementation.
