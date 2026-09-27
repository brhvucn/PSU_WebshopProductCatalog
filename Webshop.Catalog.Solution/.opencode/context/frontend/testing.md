---
domain: frontend
capabilities:
  - testing
keywords:
  - vitest
  - vue test utils
  - frontend testing
  - component test
  - pinia test
  - service mock
priority: medium
cost: low
---

# Frontend Testing

## Technology

| Layer | Test type | Tool |
|---|---|---|
| Vue components | Unit / component test | Vitest + Vue Test Utils |
| Pinia stores | Unit test | Vitest |
| Service layer | Unit test | Vitest + `vi.mock(apiService)` |

## Setup

```bash
npm install -D vitest @vue/test-utils @vitejs/plugin-vue happy-dom
```

`vitest.config.ts`:
```ts
import { defineConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  test: {
    environment: 'happy-dom',
    globals: true,
  },
})
```

## Component Test Example

```ts
// components/__tests__/ProductCard.test.ts
import { mount } from '@vue/test-utils'
import ProductCard from '../ProductCard.vue'

describe('ProductCard', () => {
  it('renders product name', () => {
    const wrapper = mount(ProductCard, {
      props: { name: 'Widget', price: 9.99 }
    })
    expect(wrapper.text()).toContain('Widget')
  })

  it('emits delete event on button click', async () => {
    const wrapper = mount(ProductCard, {
      props: { id: '1', name: 'Widget', price: 9.99 }
    })
    await wrapper.find('[data-test="delete"]').trigger('click')
    expect(wrapper.emitted('delete')).toBeTruthy()
  })
})
```

## Service Mock Example

```ts
// services/__tests__/productService.test.ts
import { vi } from 'vitest'
import apiService from '../apiService'
import productService from '../productService'

vi.mock('../apiService')

describe('productService', () => {
  it('returns products from API', async () => {
    const mockData = { result: [{ id: '1', name: 'Widget' }] }
    vi.mocked(apiService.get).mockResolvedValue(mockData)

    const result = await productService.loadAll()
    expect(result).toEqual([{ id: '1', name: 'Widget' }])
  })
})
```

## Pinia Store Test Example

```ts
// stores/__tests__/productStore.test.ts
import { setActivePinia, createPinia } from 'pinia'
import { useProductStore } from '../productStore'

describe('productStore', () => {
  beforeEach(() => setActivePinia(createPinia()))

  it('fetches products', async () => {
    const store = useProductStore()
    await store.fetchAll()
    expect(store.products.length).toBeGreaterThan(0)
  })
})
```

## Running Tests

```bash
# Run all tests
npm test

# Run with watch mode
npx vitest

# Run specific file
npx vitest run components/__tests__/ProductCard.test.ts
```

## Rules

- Mock `apiService` in all service tests — never call real endpoints
- Test component rendering and user interaction, not implementation details
- Test Pinia store actions and state transitions
- Use `data-test` attributes for test selectors — not CSS classes
- Keep tests fast — mock all external dependencies
- Place tests in `__tests__/` directories next to the code they test
