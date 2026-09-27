---
domain: backend
capabilities:
  - patterns
  - observability
keywords:
  - health check
  - IHealthCheck
  - AddHealthChecks
  - MapHealthChecks
  - readiness
  - liveness
priority: medium
cost: low
---

# Health Check Pattern

ASP.NET Core health checks verify the application and its dependencies are running.

## Setup (Program.cs)

```csharp
builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString,
        name: "database",
        tags: new[] { "ready" });

// ... after app.Build()

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = WriteHealthCheckResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteHealthCheckResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false  // no dependency checks — just "is the app running?"
});
```

## Custom Health Check

```csharp
public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnection _db;

    public DatabaseHealthCheck(IDbConnection db) => _db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.ExecuteScalarAsync<int>(
                new CommandDefinition("SELECT 1", cancellationToken: cancellationToken));
            return HealthCheckResult.Healthy("Database is reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is unreachable", ex);
        }
    }
}
```

Register:
```csharp
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" });
```

## Response Writer

```csharp
static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";
    var result = new
    {
        status = report.Status.ToString(),
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            description = e.Value.Description,
            duration = e.Value.Duration.TotalMilliseconds
        })
    };
    await context.Response.WriteAsJsonAsync(result);
}
```

## Endpoints

| Endpoint | Purpose | Response |
|---|---|---|
| `GET /health` | All checks | 200 if all healthy, 503 if any unhealthy |
| `GET /health/ready` | Dependency checks (DB, external services) | 200 / 503 |
| `GET /health/live` | Liveness only (is the process running?) | Always 200 |

## Docker Integration

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health/live"]
  interval: 10s
  timeout: 5s
  retries: 3
```

## Rules

- Always include a database health check
- `/health` must return 200 when all checks pass, 503 when any fails
- Do not expose sensitive information in health check responses
- Use tags to separate liveness from readiness checks
- Keep health checks fast — they run frequently

## NuGet Package

```bash
dotnet add package AspNetCore.HealthChecks.NpgSql
```

See also: `docker.md`.
