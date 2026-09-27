---
domain: backend
capabilities:
  - patterns
  - api
keywords:
  - openapi
  - swagger
  - swashbuckle
  - ProducesResponseType
  - xml doc
  - api documentation
priority: medium
cost: low
---

# OpenAPI / Swagger Convention

## Setup (Program.cs)

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyApp API",
        Version = "v1"
    });

    // Include XML comments from controllers
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Enable in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Enable XML doc generation in the Api `.csproj`:
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

## Controller Annotations

```csharp
/// <summary>Creates a new customer.</summary>
/// <param name="request">Customer creation data.</param>
/// <response code="200">Customer created successfully.</response>
/// <response code="400">Validation failed.</response>
/// <response code="422">Business rule violation.</response>
[HttpPost]
[ProducesResponseType(typeof(Envelope<CustomerDto>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(Envelope<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Envelope<string>), StatusCodes.Status422UnprocessableEntity)]
public async Task<IActionResult> Create(
    [FromBody] CreateCustomerRequest request,
    CancellationToken ct)
{
    var command = _mapper.Map<CreateCustomerCommand>(request);
    var result = await _mediator.Dispatch(command, ct);
    return FromResult(result);
}
```

## Standard Response Annotations

| HTTP Status | When | Response Type |
|---|---|---|
| 200 OK | Success with data | `Envelope<T>` |
| 201 Created | Resource created | `Envelope<T>` + `CreatedAtAction` |
| 204 No Content | Success without data | — |
| 400 Bad Request | Validation failure | `Envelope<string>` |
| 401 Unauthorized | Not authenticated | `ProblemDetails` |
| 403 Forbidden | Not authorized | `ProblemDetails` |
| 404 Not Found | Resource not found | `Envelope<string>` |
| 422 Unprocessable | Business rule violation | `Envelope<string>` |
| 500 Internal Error | Unhandled exception | `ProblemDetails` |

## XML Doc Rules

- All public controller actions must have `<summary>` XML doc
- All parameters should have `<param>` descriptions
- All response codes should have `<response>` descriptions
- Use `[ProducesResponseType]` for every expected status code

## JWT in Swagger

If the API uses JWT authentication:

```csharp
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = "JWT Authorization header using the Bearer scheme.",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer"
});

options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});
```

## Rules

- All public endpoints must have XML doc summary
- All response codes must be declared with `[ProducesResponseType]`
- Use `Envelope<T>` as the success response wrapper type
- Use `ProblemDetails` for auth errors (401, 403)
- Swagger UI is enabled in Development only — never in Production
- Keep endpoint descriptions concise and accurate

## NuGet Package

```bash
dotnet add package Swashbuckle.AspNetCore
```

See also: `controller-template.md`, `api-patterns.md`.
