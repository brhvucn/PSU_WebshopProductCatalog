---
domain: frontend
capabilities:
  - authentication
  - auth
keywords:
  - auth
  - login
  - logout
  - refresh token
  - auth store
  - auth service
priority: high
cost: low
---

# Authentication

Authentication uses a two-layer separation:

1. **authService.js** — API calls (login, refresh, me, register)
2. **authStore.js (Pinia)** — state + actions (tokens, user, login/logout flow)

## Auth Service

Handles HTTP communication with auth endpoints.

```js
import apiService from './apiService'
import endpoints from './endpoints'

export default {
  async login(username, password) { /* ... */ },
  async refresh(refreshToken) { /* ... */ },
  async me() { /* ... */ },
  async logout() { /* ... */ },
}
```

## Auth Store (Pinia)

Manages auth state: `user`, `authToken`, `refreshToken`, `loading`.

Actions:
- `login(username, password)` — calls service, stores tokens
- `logout()` — clears tokens
- `refresh()` — refreshes token via service
- `fetchMe()` — loads current user

Tokens are persisted in `localStorage` under `auth_token` and `refresh_token`.

## Flow

```
Login view → authStore.login()
  → authService.login()
    → apiService.post(endpoints.AUTH.LOGIN(), data)
  → store tokens
  → redirect to home
```

See example files for full implementation:
- `.opencode/context/examples/frontend-authService.js.md`
