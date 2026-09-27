---
domain: backend
capabilities:
  - patterns
  - security
keywords:
  - authentication
  - authorization
  - jwt
  - bearer
  - authorize
  - claims
  - ICurrentUserService
  - policy
priority: high
cost: low
---

# Authentication & Authorization Pattern

## JWT Setup (Program.cs)

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
```

Middleware order in `Program.cs`:
```csharp
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

## ICurrentUserService

Interface in **Application** layer. Implementation in **Api** layer (reads `HttpContext`).
Inject into handlers that need user identity.

```csharp
// Application/Contracts/ICurrentUserService.cs
public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

// Api/Services/CurrentUserService.cs
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User
        .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?.User
        .FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User
        .Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User
        .IsInRole(role) ?? false;
}
```

DI registration:
```csharp
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
```

## Authorization on Controllers

```csharp
[Authorize]                          // requires any authenticated user
[Authorize(Roles = "Admin")]         // requires Admin role
[Authorize(Policy = "CanEditOrders")]// requires named policy
[AllowAnonymous]                     // overrides [Authorize] on controller
```

## Named Policies

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditOrders", policy =>
        policy.RequireClaim("permission", "orders.edit"));
});
```

## Error Mapping

| Situation | Result |
|---|---|
| No token / invalid token | 401 Unauthorized (automatic by middleware) |
| Valid token, insufficient permissions | 403 Forbidden (automatic by middleware) |
| Business rule: user not allowed | `Result.Fail(Errors.General.Unauthorized())` |
| Business rule: resource not owned | `Result.Fail(Errors.General.Forbidden())` |

## Configuration

Store JWT settings in `appsettings.json` (non-secret values only):
```json
{
  "Jwt": {
    "Issuer": "MyApp",
    "Audience": "MyApp",
    "ExpiryMinutes": 60
  }
}
```

**Secret key** must come from environment variables — never in `appsettings.json`:
```
JWT_KEY=your-256-bit-secret-key-here
```

## Rules

- Never read `HttpContext` inside handlers — use `ICurrentUserService`
- Never store secrets in `appsettings.json` or source code — use environment variables
- Always validate token expiry, issuer and audience
- Use `[Authorize]` on controllers or actions — not custom middleware
- Use named policies for fine-grained authorization
- Map authorization failures to `Result.Fail()` for business-level denials

## NuGet Package

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

See also: `error-handling.md`, `cross-cutting-concerns.md`.
