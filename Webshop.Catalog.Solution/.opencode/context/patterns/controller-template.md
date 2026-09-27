---
domain: backend
capabilities:
  - patterns
keywords:
  - controller
  - template
  - services
  - api
priority: medium
cost: low
---

# Controller Template

Controllers are thin HTTP adapters. Must not contain business logic or data access.

## Example (Using Exceptions)

```csharp
[ApiController]
[Route("api/[controller]")]
public sealed class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id, ct);
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketRequest request,
        CancellationToken ct)
    {
        var ticketId = await _ticketService.CreateTicketAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = ticketId }, new { id = ticketId });
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateStatusRequest request,
        CancellationToken ct)
    {
        await _ticketService.UpdateTicketStatusAsync(id, request.Status, ct);
        return NoContent();
    }
}
```

## Example (Using Result<T> Pattern)

```csharp
[ApiController]
[Route("api/[controller]")]
public sealed class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _ticketService.GetTicketByIdAsync(id, ct);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketRequest request,
        CancellationToken ct)
    {
        var result = await _ticketService.CreateTicketAsync(request, ct);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }
}
```

## Naming

| Type | Convention |
|------|-----------|
| Controller | `TicketsController` |
| Request DTO | `CreateTicketRequest` |
| Response DTO | `TicketDto` |
| Service | `TicketService` |
| Service Interface | `ITicketService` |

## Rules

- Controllers delegate to services — no business logic
- Inject service interfaces (e.g., `ITicketService`)
- Use request DTOs for write operations
- Map exceptions to appropriate HTTP status codes (use exception filter/middleware)
- Or use Result<T> pattern and map results to status codes
- Return standardized responses (Ok, Created, NoContent, BadRequest, NotFound)
- No repositories, SQL, business logic, or orchestration in controllers

See also: `service-template.md`, `result-pattern.md`.
