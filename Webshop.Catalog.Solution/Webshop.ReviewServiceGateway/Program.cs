using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
//add Ocelot Config
//builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddJsonFile("ocelotloadbalancer.json");
//builder.Configuration.AddJsonFile("ocelotrequestaggregator.json");
//add Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add healthchecks
builder.Services.AddHealthChecks();
var app = builder.Build();


//always show swagger
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
//enable prometheus metrics
app.UseHttpMetrics();
app.UseMetricServer();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

app.MapGet("/", () => $"Gateway: ReviewService");
//add ocelot middleware
app.UseOcelot().Wait(); //ocelot must be the last thing to add
app.Run();
