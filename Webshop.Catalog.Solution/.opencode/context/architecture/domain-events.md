---
domain: backend
capabilities:
  - architecture
keywords:
  - domain events
  - business events
priority: high
cost: low
---

# Domain Events

Domain events represent important business events that occurred inside the domain.

Examples:

- CustomerCreatedEvent
- OrderPaidEvent
- SubscriptionExpiredEvent

## Purpose

Domain events are used to:

- decouple business reactions
- trigger side effects
- coordinate workflows
- publish integration events later if needed

## Rules

- Domain events represent past-tense business facts.
- Domain events should be immutable where practical.
- Domain events should not contain infrastructure concerns.
- Domain events should not perform work directly.

## Structure

```txt
DomainEvents/
├── IDomainEvent.cs
└── Events/
```

## Dispatching

Events are dispatched through IDomainEventDispatcher.

Infrastructure contains the concrete dispatcher implementation.

### Handler rules

Domain event handlers should:

- react to events
- remain independent
- avoid tight coupling

Handlers should not:

- depend on controllers
- contain UI concerns
- perform unrelated orchestration

## Naming Conventions

|Type|Convention|
|---|---|
|Event|`CustomerCreatedEvent` or `[Entity][Action]Event`|
|Handler|`CustomerCreatedEventHandler` or `[Event]Handler`|

## Important Principle

Domain events describe something that already happened.

Do not use them as commands.