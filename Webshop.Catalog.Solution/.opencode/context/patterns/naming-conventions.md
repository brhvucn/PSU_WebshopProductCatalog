---
domain: conventions
capabilities:
  - patterns
keywords:
  - naming conventions
  - naming
  - conventions
  - pascalcase
  - snake_case
  - _camelcase
priority: medium
cost: low
---

# Naming Conventions

| Concept | Convention | Example |
|---|---|---|
| Commands | `[Verb][Entity]Command` | `CreateOrderCommand` |
| Queries | `Get[Entity]Query` | `GetOrderByIdQuery` |
| Handlers | `[Command/Query]Handler` | `CreateOrderCommandHandler` |
| Repositories | `I[Entity]Repository` | `IOrderRepository` |
| DTOs | `[Entity][Purpose]Dto` | `OrderSummaryDto` |
| Error classes | `[Entity]Errors` | `OrderErrors` |
| DB tables | `snake_case` | `order_lines` |
| C# properties | `PascalCase` | `CustomerId` |
| Private fields | `_camelCase` | `_orderRepository` |
