---
domain: backend
capabilities:
  - patterns
  - observability
keywords:
  - serilog
  - logging
  - structured logging
  - correlation id
  - log level
  - observability
priority: medium
cost: low
---

# Structured Logging Pattern

Framework: **Serilog** via `Serilog.AspNetCore`.

## Setup (Program.cs)

```csharp
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationId()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}"));
```

## Configuration (appsettings.json)

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## Log Levels

| Level | When to use |
|---|---|
| `Debug` | Internal state during development — never in production |
| `Information` | Significant business events (order created, user logged in) |
| `Warning` | Recoverable issues (retry attempted, fallback used, slow query) |
| `Error` | Failures requiring attention (handler failed, DB connection error) |
| `Fatal` | Application cannot continue (startup failure, critical config missing) |

## Structured Properties

Always use structured log properties — never string interpolation:

```csharp
// GOOD — structured, searchable
Log.Information("Order {OrderId} created for customer {CustomerId}", orderId, customerId);

// BAD — unstructured, not searchable
Log.Information($"Order {orderId} created for customer {customerId}");
```

## Correlation ID Middleware

Add correlation ID to every HTTP request for traceability:

```csharp
// Middleware/CorrelationIdMiddleware.cs
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
```

Register in `Program.cs`:
```csharp
app.UseMiddleware<CorrelationIdMiddleware>();
```

## Rules

- Never log passwords, tokens, connection strings or PII
- Always include correlation ID on HTTP requests
- Use structured properties — never string interpolation in log messages
- Log at `Information` level for business events, `Error` for failures
- Do not duplicate logging inside handlers — use the logging pipeline behavior
- Configure log levels per namespace to reduce noise

## NuGet Packages

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Enrichers.CorrelationId
```

See also: `cross-cutting-concerns.md`.
