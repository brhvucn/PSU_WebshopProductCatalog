---
domain: frontend
capabilities:
  - state-management
  - pinia
keywords:
  - pinia
  - store
  - state management
priority: medium
cost: low
---

# Pinia Stores

Pinia manages shared client state: login status, user data, settings, or cross-page data.

## Principles

- Stores hold state and actions, not large UI logic.
- Services handle API calls; stores call services.
- Components read store state reactively.

## Example: Auth Store

```js
import { defineStore } from 'pinia'
import authService from '@/services/authService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    authToken: localStorage.getItem('auth_token'),
    refreshToken: localStorage.getItem('refresh_token'),
    loading: false,
  }),

  getters: {
    isAuthenticated: (state) => !!state.authToken,
  },

  actions: {
    async login(username, password) { /* calls authService */ },
    async logout() { /* clears state + storage */ },
    async refresh() { /* refreshes token */ },
  },
})
```

## Rules

- Do not put endpoint strings or Axios calls in stores.
- Do not use stores as replacement for all local component data.
- Keep stores focused on one concern.
