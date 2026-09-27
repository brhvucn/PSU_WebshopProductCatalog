---
title: Minimal API Testing
capability: minimal-api, testing, webapplicationfactory, integration-tests
keywords: testing, integration-tests, webapplicationfactory, xunit
type: pattern
token-estimate: 380
---

# Minimal API Testing

Integration tests using WebApplicationFactory.

## Test Project Setup

```xml
<!-- Tests.csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.0" />
  <PackageReference Include="xunit" Version="2.9.3" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\YourApi\YourApi.csproj" />
</ItemGroup>
```

## Test Class

```csharp
// Tests/TicketApiTests.cs
public class TicketApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TicketApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTickets_ReturnsOkWithList()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var tickets = await response.Content
            .ReadFromJsonAsync<List<TicketDto>>();
        
        Assert.NotNull(tickets);
    }

    [Fact]
    public async Task CreateTicket_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new CreateTicketRequest 
        { 
            Title = "Test Ticket",
            Description = "Test description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var ticket = await response.Content.ReadFromJsonAsync<TicketDto>();
        Assert.NotNull(ticket);
        Assert.Equal("Test Ticket", ticket.Title);
    }

    [Fact]
    public async Task GetTicket_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets/999999");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
```

## Custom WebApplicationFactory

```csharp
// Tests/CustomWebApplicationFactory.cs
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace real database with in-memory for tests
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDbConnectionFactory));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IDbConnectionFactory>(
                new InMemoryConnectionFactory());
        });
    }
}
```

## Rules

- Use `WebApplicationFactory<Program>` for integration tests
- Test full request/response cycle
- Mock/replace database with in-memory for tests
- Test happy path AND error scenarios
- Use `IClassFixture` to share factory across tests
- Program.cs must expose `public partial class Program { }` for testing
