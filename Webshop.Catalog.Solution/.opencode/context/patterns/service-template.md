---
domain: backend
capabilities:
  - services
  - business-logic
keywords:
  - service
  - business logic
  - orchestration
priority: high
cost: medium
---

# Service Template

## Purpose

Services contain business logic, validation, and orchestration. They coordinate repositories and implement business workflows.

## Template

```csharp
using [SolutionName].Business.Interfaces;
using [SolutionName].Data.Interfaces;

namespace [SolutionName].Business.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(
        ITicketRepository ticketRepository,
        IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<TicketDto> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        // Option 1: Using exceptions (standard .NET)
        var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with ID {ticketId} not found");
        }

        return MapToDto(ticket);
    }

    public async Task<Guid> CreateTicketAsync(CreateTicketRequest request, CancellationToken cancellationToken = default)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ValidationException("Title is required");
        }

        // Business logic
        var user = await _userRepository.GetByIdAsync(request.AssignedUserId, cancellationToken);
        if (user == null)
        {
            throw new ValidationException($"User {request.AssignedUserId} does not exist");
        }

        // Create entity
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            AssignedUserId = request.AssignedUserId,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        // Persist
        await _ticketRepository.AddAsync(ticket, cancellationToken);

        return ticket.Id;
    }

    public async Task UpdateTicketStatusAsync(Guid ticketId, TicketStatus newStatus, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket {ticketId} not found");
        }

        // Business rule validation
        if (ticket.Status == TicketStatus.Closed)
        {
            throw new BusinessRuleException("Cannot change status of closed ticket");
        }

        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
    }

    private TicketDto MapToDto(Ticket ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            AssignedUserId = ticket.AssignedUserId,
            CreatedAt = ticket.CreatedAt
        };
    }
}
```

## Alternative: Result<T> Pattern

```csharp
public async Task<Result<TicketDto>> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
{
    var ticket = await _ticketRepository.GetByIdAsync(ticketId, cancellationToken);
    
    if (ticket == null)
    {
        return Result<TicketDto>.Failure($"Ticket {ticketId} not found");
    }

    var dto = MapToDto(ticket);
    return Result<TicketDto>.Success(dto);
}

public async Task<Result<Guid>> CreateTicketAsync(CreateTicketRequest request, CancellationToken cancellationToken = default)
{
    // Validation
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Result<Guid>.Failure("Title is required");
    }

    // Business logic
    var ticket = new Ticket { /* ... */ };

    await _ticketRepository.AddAsync(ticket, cancellationToken);

    return Result<Guid>.Success(ticket.Id);
}
```

## Rules

- Services coordinate repositories
- Services contain business logic and validation
- Services should not know about HTTP, controllers, or DTOs (receive/return business models)
- One service per aggregate or feature area
- Methods should have clear, intention-revealing names
- Use async/await for all I/O operations
- Accept CancellationToken for all async methods
