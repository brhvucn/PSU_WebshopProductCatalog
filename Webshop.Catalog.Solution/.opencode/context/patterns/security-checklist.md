---
domain: backend
capabilities:
  - security
  - patterns
keywords:
  - security
  - owasp
  - sql injection
  - xss
  - cors
  - secrets
  - authorization
  - input validation
priority: medium
cost: low
---

# Security Checklist for .NET APIs

## OWASP Top 10 Relevance

| OWASP Risk | .NET Mitigation |
|---|---|
| A01 Broken Access Control | `[Authorize]` attributes, named policies, `ICurrentUserService` |
| A02 Cryptographic Failures | Environment variables for secrets, HTTPS only, no hardcoded keys |
| A03 Injection | Dapper parameterized queries, FluentValidation on all inputs |
| A04 Insecure Design | 3-layer architecture boundaries, proper error handling (exceptions or Result<T>) |
| A05 Security Misconfiguration | Swagger disabled in production, CORS restricted, no default credentials |
| A06 Vulnerable Components | Regular NuGet package updates, `dotnet list package --vulnerable` |
| A07 Auth Failures | JWT with expiry/issuer/audience validation, refresh token rotation |
| A08 Data Integrity Failures | Input validation, domain invariants, no deserialization of untrusted data |
| A09 Logging Failures | Serilog structured logging, correlation IDs, no PII in logs |
| A10 SSRF | No user-controlled URLs in server-side HTTP calls |

## SQL Injection Prevention

```csharp
// SAFE — parameterized
await conn.QueryAsync<Customer>("SELECT * FROM customers WHERE id = @Id", new { Id = id });

// UNSAFE — string interpolation
await conn.QueryAsync<Customer>($"SELECT * FROM customers WHERE id = '{id}'");
```

## CORS Configuration

```csharp
// GOOD — restrictive
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://myapp.com")
              .AllowAnyHeader()
              .AllowAnyMethod()));

// BAD — wildcard in production
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
```

## Secret Management

| Environment | Method |
|---|---|
| Development | `appsettings.Development.json` + user secrets |
| CI/CD | GitHub Secrets / Azure Key Vault |
| Production | Environment variables / cloud secret manager |

Never commit: connection strings, JWT keys, API keys, passwords.

## Error Response Safety

```csharp
// GOOD — generic error, no internals exposed
return StatusCode(500, Envelope.Error("An unexpected error occurred."));

// BAD — stack trace exposed
return StatusCode(500, Envelope.Error(exception.ToString()));
```

## Input Validation

All public endpoints must validate input via FluentValidation:
- String length limits
- Required fields
- Format validation (email, phone, etc.)
- Range validation (amounts, quantities)

## Rules

- Run `dotnet list package --vulnerable` regularly
- Never trust client-side validation alone — always validate server-side
- Use `[Authorize]` by default — use `[AllowAnonymous]` only with justification
- Log security events (failed auth, access denied) at Warning level
- Review CORS policy before every production deployment

See also: `authentication.md`, `error-handling.md`, `logging-pattern.md`.
