---
domain: frontend
capabilities:
  - layout
  - bulma
  - ui
keywords:
  - bulma
  - layout
  - app.vue
  - sidebar
  - header
  - shell
priority: high
cost: low
---

# UI Layout

The app uses Bulma for layout and styling with custom CSS for the application shell.

## App Shell

`App.vue` composes Sidebar + Header + `<router-view />`:

```vue
<template>
  <div id="app">
    <SidebarComponent v-if="route.meta.layout !== 'auth'" ... />
    <main class="main-content">
      <HeaderComponent v-if="route.meta.layout !== 'auth'" />
      <router-view />
    </main>
  </div>
</template>
```

Auth pages (login) use a minimal layout without sidebar or header.

## Sidebar

- Fixed left sidebar (240px, dark background `#1e2a38`).
- Collapsible to 80px (icons only).
- Shows contextual navigation based on organization context.
- Includes logout button.
- Overlay on mobile (responsive: hidden by default, slides in via transform).

## Header

Sticky top bar with burger menu button for mobile sidebar toggle.

## Title Component

Reusable header with optional "Add new" button:

```vue
<TitleComponent title="Organizations" :showAdd="true" @addElement="openCreate" />
```

## Bulma Usage

- Use Bulma classes before custom CSS.
- Common: `button`, `level`, `title`, `notification`, `columns`, `box`, `form` elements.
- Custom CSS reserved for application-specific layout (sidebar, header shell).

See example files for full implementations:
- `.opencode/context/examples/frontend-SidebarComponent.vue.md`
- `.opencode/context/examples/frontend-HeaderComponent.vue.md`
- `.opencode/context/examples/frontend-TitleComponent.vue.md`
