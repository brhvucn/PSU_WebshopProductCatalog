using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using System.Reflection;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Customer.Application;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Customer.Persistence;
using Webshop.Data.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Webshop.Customer.Api", Version = "v1" });
});

// Add custom services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<DataContext, DataContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
builder.Services.AddCustomerApplicationServices();

// Add health checks
builder.Services.AddHealthChecks();

// Configure Serilog
var configuration = builder.Configuration;
string seqUrl = configuration.GetValue<string>("Settings:SeqLogAddress");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Customer.API")
    .WriteTo.Seq(seqUrl)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webshop.Customer.Api v1"));

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Enable Prometheus metrics
app.UseHttpMetrics();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

app.Run();
