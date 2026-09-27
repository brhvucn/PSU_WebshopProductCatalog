---
domain: backend
capabilities:
  - patterns
keywords:
  - validation
  - fluentvalidation
  - data annotations
priority: medium
cost: low
---

# Validation Rules

## Validation Options

**Option 1: Data Annotations (simplest)**
```csharp
public class CreateTicketRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}
```

**Option 2: FluentValidation (recommended for complex validation)**
```csharp
public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);
            
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
```

**Option 3: Manual validation in services**
```csharp
public async Task<Guid> CreateTicketAsync(CreateTicketRequest request, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        throw new ValidationException("Title is required");
    }
    
    // ... business logic
}
```

## Principles

- Validation validates input format and basic rules
- Business logic validates business invariants and workflows
- Validation should fail fast
- Validation should be deterministic
- Validation should not mutate state

## Placement

- DTOs: Data Annotations for simple validation
- Validators: FluentValidation classes in Business layer
- Services: Manual validation for complex business rules

## Anti-patterns

Avoid:

- complex business workflows inside validators
- SQL-heavy validators
- validators with side effects
- duplicated validation logic across layers