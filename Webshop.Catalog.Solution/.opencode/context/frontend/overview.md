---
domain: frontend
capabilities:
  - architecture
  - overview
keywords:
  - vue 3
  - frontend structure
  - tech stack
priority: high
cost: low
---

# Frontend Overview

## Technology Stack

- **Vue 3** — component framework
- **Pinia** — state management
- **Vue Router** — navigation
- **Bulma** — layout and styling
- **Axios** — HTTP client
- **Toastr** — user notifications

## Folder Structure

```
src/
  assets/
  components/       reusable UI components
  layouts/          layout components
  views/            pages connected to routes
  router/index.js   route definitions
  stores/           Pinia stores
  services/         API calls and integrations
    apiService.js   Axios config
    endpoints.js    endpoint paths
    authService.js
    userService.js
  App.vue
  main.js
```

## Core Principles

1. Components and views call **services**, not Axios directly.
2. `apiService.js` centralizes Axios config, base URL, token handling and interceptors.
3. `endpoints.js` collects all endpoint strings in one file.
4. Pinia stores manage shared state (auth, user, settings).
5. Bulma classes are preferred before custom CSS.
6. Toastr provides success/error feedback.
7. Components stay small and focused.

## Standard API Call Flow

```
View/Component
  → Pinia store or service
  → Service
  → apiService (Axios)
  → API endpoint
```

Views may use services directly but must not know concrete endpoint strings.

## Discover Related Files

Use context search to find example implementations:

```powershell
.opencode\scripts\context-manager.ps1 discover -Task "frontend examples" -MaxTokens 4000
```
