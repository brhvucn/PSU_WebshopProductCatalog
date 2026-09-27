---
domain: frontend
capabilities:
  - feedback
  - notifications
keywords:
  - toastr
  - notifications
  - loading
  - user feedback
priority: low
cost: low
---

# User Feedback

## Toastr Notifications

Toastr provides user feedback on success and error.

```js
import toastr from 'toastr'

try {
  await service.save(data)
  toastr.success('Data was saved')
} catch (error) {
  toastr.error(error?.message || 'An error occurred')
}
```

Typical use cases: data saved, API error, login success/failure, validation messages.

## Loading Banner

`LoadingBannerComponent.vue` shows a loading indicator during async operations.

```vue
<LoadingBannerComponent :visible="isLoading" message="Loading data..." />
```

Features: fixed top-center position, fold-down animation, auto-hides when `visible` is false.

See example: `.opencode/context/examples/frontend-LoadingBannerComponent.vue.md`
