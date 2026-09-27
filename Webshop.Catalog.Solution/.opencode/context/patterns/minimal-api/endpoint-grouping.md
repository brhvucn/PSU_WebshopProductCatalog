---
title: Minimal API Endpoint Grouping
capability: minimal-api, endpoints, routing, grouping
keywords: minimal-api, map-group, route-prefix, endpoint-filter
type: pattern
token-estimate: 450
---

# Minimal API Endpoint Grouping

Map endpoints with route prefixes and shared filters.

## Pattern

```csharp
// Program.cs
var app = builder.Build();

var tickets = app.MapGroup("/api/tickets")
    .WithTags("Tickets")
    .RequireAuthorization();

tickets.MapGet("/", GetAllTickets);
tickets.MapGet("/{id:int}", GetTicketById);
tickets.MapPost("/", CreateTicket);
tickets.MapPut("/{id:int}", UpdateTicket);
tickets.MapDelete("/{id:int}", DeleteTicket);

app.Run();
```

## Endpoint Handlers

```csharp
// Handlers/TicketHandlers.cs
public static class TicketHandlers
{
    public static async Task<IResult> GetAllTickets(
        ITicketService ticketService)
    {
        var tickets = await ticketService.GetAllAsync();
        return Results.Ok(tickets);
    }

    public static async Task<IResult> GetTicketById(
        int id, 
        ITicketService ticketService)
    {
        var ticket = await ticketService.GetByIdAsync(id);
        return ticket is not null 
            ? Results.Ok(ticket) 
            : Results.NotFound();
    }

    public static async Task<IResult> CreateTicket(
        CreateTicketRequest request,
        ITicketService ticketService)
    {
        var ticket = await ticketService.CreateAsync(request);
        return Results.Created($"/api/tickets/{ticket.Id}", ticket);
    }
}
```

## Rules

- Group related endpoints with `MapGroup`
- Extract handlers to static classes in `Handlers/` folder
- Use typed route constraints (`{id:int}`)
- Apply filters at group level (auth, validation, etc.)
- Use `WithTags` for OpenAPI documentation
- Return `IResult` from handlers (Results.Ok, Results.NotFound, etc.)
