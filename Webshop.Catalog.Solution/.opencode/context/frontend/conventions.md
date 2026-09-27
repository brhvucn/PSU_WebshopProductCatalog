---
domain: frontend
capabilities:
  - conventions
  - rules
keywords:
  - conventions
  - rules
  - best practices
priority: medium
cost: low
---

# Frontend Conventions

## API Calls

- Components must not call Axios directly. Use services.
- Endpoint strings must be in `endpoints.js`.
- Axios configuration must be in `apiService.js`.
- Use Axios interceptors for token, refresh-token, and 401 handling.

## State Management

- Pinia for shared state (auth, user, settings).
- Not a replacement for all local component data.
- Services handle API calls; stores coordinate state and actions.

## Styling

- Use Bulma classes before custom CSS.
- Custom CSS reserved for app-specific layout (sidebar, header shell).

## Components

- Keep small and focused.
- Display and user interaction only; data fetching delegated to services/stores.
- Use Bulma form elements, buttons, columns before custom styling.

## Error Handling

- Show user-friendly errors via Toastr.
- Handle API errors in service layer where possible.
- Never expose raw error messages to users.
