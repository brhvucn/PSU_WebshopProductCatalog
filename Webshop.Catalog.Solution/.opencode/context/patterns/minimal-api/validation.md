---
title: Minimal API Validation
capability: minimal-api, validation, endpoint-filter
keywords: validation, endpoint-filter, problem-details, fluent-validation
type: pattern
token-estimate: 380
---

# Minimal API Validation

Validate requests using endpoint filters and FluentValidation.

## Endpoint Filter Pattern

```csharp
// Filters/ValidationFilter.cs
public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (request is null)
            return Results.BadRequest("Request body required");

        var result = await _validator.ValidateAsync(request);
        
        if (!result.IsValid)
            return Results.ValidationProblem(result.ToDictionary());

        return await next(context);
    }
}
```

## Validator Example

```csharp
// Validators/CreateTicketRequestValidator.cs
public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);
    }
}
```

## Usage

```csharp
// Program.cs - Register validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Apply filter to endpoint
tickets.MapPost("/", CreateTicket)
    .AddEndpointFilter<ValidationFilter<CreateTicketRequest>>();
```

## Rules

- Use FluentValidation for validation logic
- Register validators in DI container
- Apply filter at endpoint level
- Return ValidationProblem for validation errors (400 + ProblemDetails)
- Validator classes in `Validators/` folder
