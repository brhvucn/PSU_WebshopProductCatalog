---
title: Minimal API Error Handling
capability: minimal-api, error-handling, middleware, exception-handler
keywords: error-handling, exception-handler, problem-details, middleware
type: pattern
token-estimate: 450
---

# Minimal API Error Handling

Global exception handling with ProblemDetails.

## Exception Handler Middleware

```csharp
// Program.cs
var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionFeature = context.Features
            .Get<IExceptionHandlerFeature>();
        
        var exception = exceptionFeature?.Error;

        var problemDetails = exception switch
        {
            ValidationException validationEx => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = validationEx.Message,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = notFoundEx.Message,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
            },
            UnauthorizedAccessException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Authentication required",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
            }
        };

        context.Response.StatusCode = problemDetails.Status ?? 500;
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.Run();
```

## Custom Exceptions

```csharp
// Exceptions/NotFoundException.cs
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Exceptions/ValidationException.cs
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
```

## Usage in Services

```csharp
public async Task<Ticket> GetByIdAsync(int id)
{
    var ticket = await _repository.GetByIdAsync(id);
    
    if (ticket is null)
        throw new NotFoundException($"Ticket with ID {id} not found");
    
    return ticket;
}
```

## Rules

- Use `UseExceptionHandler` middleware for global exception handling
- Return ProblemDetails format (RFC 9110 compliant)
- Map exceptions to appropriate HTTP status codes
- Log exceptions before returning response (use ILogger)
- Custom exceptions in `Exceptions/` folder
- Never expose stack traces in production
