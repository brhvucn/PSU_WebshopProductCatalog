---
domain: backend
capabilities:
  - patterns
keywords:
  - repository
  - template
  - dapper
priority: medium
cost: low
---

# Repository Template

Repositories encapsulate persistence in the Infrastructure layer.

## Base Repository

```csharp
public abstract class BaseRepository
{
    private readonly IConfiguration configuration;

    protected BaseRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(
            this.configuration.GetConnectionString("DefaultConnection"));
    }
}
```

## Example

```csharp
public sealed class RubricRepository : BaseRepository, IRubricRepository
{
    public RubricRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<Rubric> CreateAsync(Rubric rubric, CancellationToken ct)
    {
        const string sql = """
            INSERT INTO rubrics (id, title, description)
            VALUES (@Id, @Title, @Description)
            RETURNING id, title, description
            """;
        using var conn = CreateConnection();
        return await conn.QuerySingleAsync<Rubric>(
            new CommandDefinition(sql, rubric, cancellationToken: ct));
    }
}
```

Place interfaces in `Business/Interfaces/` or `Data/Interfaces/`.

## DI

```csharp
services.AddScoped<IRubricRepository, RubricRepository>();
```

## Naming

| Type | Convention |
|------|-----------|
| Repository | `RubricRepository` |
| Interface | `IRubricRepository` |
| Base | `BaseRepository` |
| Table | `rubrics` |
| Column | `created_utc` |

## Transaction Pattern

For multi-step write operations that must be atomic:

```csharp
public async Task<Result> CreateOrderWithLinesAsync(
    Order order,
    IReadOnlyList<OrderLine> lines,
    CancellationToken ct)
{
    using var conn = CreateConnection();
    await conn.OpenAsync(ct);
    using var tx = await conn.BeginTransactionAsync(ct);
    try
    {
        const string orderSql = """
            INSERT INTO orders (id, customer_id, total, created_utc)
            VALUES (@Id, @CustomerId, @Total, @CreatedUtc)
            """;
        await conn.ExecuteAsync(new CommandDefinition(orderSql, order, tx, cancellationToken: ct));

        const string lineSql = """
            INSERT INTO order_lines (id, order_id, product_id, quantity, price)
            VALUES (@Id, @OrderId, @ProductId, @Quantity, @Price)
            """;
        foreach (var line in lines)
            await conn.ExecuteAsync(new CommandDefinition(lineSql, line, tx, cancellationToken: ct));

        await tx.CommitAsync(ct);
        return Result.Ok();
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync(ct);
        return Result.Fail(Errors.Database.CouldNotInsert(ex.Message));
    }
}
```

## Rules

- Repository interfaces are defined in **Data/Interfaces**, implementations in **Data/Repositories**
- Use **Dapper** for all queries — do not introduce Entity Framework Core
- Use explicit parameterized SQL — never string interpolation in SQL
- Use Dapper async methods (`QueryAsync`, `ExecuteAsync`) — never synchronous variants
- Use `RETURNING` for PostgreSQL inserts
- Repositories return data models — never expose `IDbConnection` outside Data layer
- Open/close connections per operation
- Use explicit `IDbTransaction` for multi-step writes — never implicit transactions
- No business logic, HTTP concerns, or orchestration

See also: `data-access.md` in `techstack/` for database technology specs.
