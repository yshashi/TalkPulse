using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using TalkPulse.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ───────────────────────────────────────────────────────────────────
// Writes structured logs to Console and to the Aspire dashboard via OTLP.
builder.Host.UseSerilog((context, services, lc) =>
{
    lc.ReadFrom.Configuration(context.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext()
      .WriteTo.Console(
          outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
      .WriteTo.OpenTelemetry(options =>
      {
          options.ResourceAttributes = new Dictionary<string, object>
          {
              ["service.name"] = context.HostingEnvironment.ApplicationName,
              ["service.version"] = "1.0.0"
          };
      });
});

// ── Aspire service defaults (OpenTelemetry metrics + tracing, health checks) ──
builder.AddServiceDefaults();

// ── Database: EF Core + PostgreSQL ───────────────────────────────────────────
// "talkpulsedb" must match the database name registered in AppHost.
builder.AddNpgsqlDbContext<AppDbContext>("talkpulsedb");

// ── Messaging: RabbitMQ ───────────────────────────────────────────────────────
// "messaging" must match the resource name registered in AppHost.
builder.AddRabbitMQClient("messaging");

// ── OpenAPI ───────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Health check endpoints: /health and /alive
app.MapDefaultEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Generating weather forecast");
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    logger.LogInformation("Weather forecast generated: {@Forecast}", forecast);
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

