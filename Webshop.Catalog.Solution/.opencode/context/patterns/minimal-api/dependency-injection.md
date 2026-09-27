---
title: Minimal API Dependency Injection
capability: minimal-api, dependency-injection, service-registration
keywords: minimal-api, di, service-registration, scoped, singleton
type: pattern
token-estimate: 300
---

# Minimal API Dependency Injection

Register and inject services in Minimal API.

## Service Registration

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Business services
builder.Services.AddScoped<ITicketService, TicketService>();

// Repositories
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Database connection factory
builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new NpgsqlConnectionFactory(
        builder.Configuration.GetConnectionString("Default")));

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Learn more about configuring OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

## Injection in Handlers

```csharp
// Handlers/TicketHandlers.cs
public static class TicketHandlers
{
    public static async Task<IResult> CreateTicket(
        CreateTicketRequest request,
        ITicketService ticketService,        // Injected
        IValidator<CreateTicketRequest> validator) // Injected
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var ticket = await ticketService.CreateAsync(request);
        return Results.Created($"/api/tickets/{ticket.Id}", ticket);
    }
}
```

## Rules

- Inject dependencies as method parameters (not constructor)
- Use `AddScoped` for per-request services (repositories, business services)
- Use `AddSingleton` for shared state (connection factories, configuration)
- Use `AddTransient` for lightweight stateless services
- Register all validators with `AddValidatorsFromAssemblyContaining<T>()`
