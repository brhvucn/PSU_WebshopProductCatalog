---
domain: frontend
capabilities:
  - routing
  - navigation
keywords:
  - vue router
  - routes
  - navigation guard
priority: medium
cost: low
---

# Vue Router

Vue Router defines navigation between pages. Routes point to views.

## Route Configuration

```js
const routes = [
  { path: '/', name: 'home', component: DashboardView, meta: { requiresAuth: true } },
  { path: '/login', name: 'login', component: LoginView, meta: { layout: 'auth' } },
  { path: '/organizations', name: 'organizations', component: OrganizationsView, meta: { requiresAuth: true } },
]
```

## Navigation Guard

A `beforeEach` guard redirects unauthenticated users to `/login`:

```js
router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return '/login'
  }
})
```

## Meta Fields

| Field | Purpose |
|---|---|
| `requiresAuth` | Route requires authentication |
| `layout` | `'auth'` hides sidebar/header for login pages |
| `organizationContext` | Route is within an organization scope |
