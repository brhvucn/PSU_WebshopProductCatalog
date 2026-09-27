---
domain: backend
capabilities:
  - patterns
keywords:
  - endpoint
  - template
  - controller
  - services
priority: medium
cost: low
---

# Endpoint Template

## POST

```csharp
[HttpPost]
public async Task<IActionResult> Create(
    [FromBody] CreateTicketRequest request,
    CancellationToken ct)
{
    var ticketId = await _ticketService.CreateTicketAsync(request, ct);
    return CreatedAtAction(nameof(GetById), new { id = ticketId }, new { id = ticketId });
}
```

## PUT

```csharp
[HttpPut("{id:guid}")]
public async Task<IActionResult> Update(
    Guid id, [FromBody] UpdateTicketRequest request, CancellationToken ct)
{
    await _ticketService.UpdateTicketAsync(id, request, ct);
    return NoContent();
}
```

## DELETE

```csharp
[HttpDelete("{id:guid}")]
public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
{
    await _ticketService.DeleteTicketAsync(id, ct);
    return NoContent();
}
```

## GET (Single)

```csharp
[HttpGet("{id:guid}")]
public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
{
    var ticket = await _ticketService.GetTicketByIdAsync(id, ct);
    return Ok(ticket);
}
```

## GET (List)

```csharp
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
{
    var tickets = await _ticketService.GetAllTicketsAsync(page, pageSize, ct);
    return Ok(tickets);
}
```

## Rules

- Endpoints must remain thin — no business logic
- Always delegate to services
- Use appropriate HTTP status codes (Ok, Created, NoContent, BadRequest, NotFound)
- Forward `CancellationToken` through pipeline
- No SQL, repositories, or business orchestration in controllers
