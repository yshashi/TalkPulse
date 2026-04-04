using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using TalkPulse.Api.Common.Domains;
using TalkPulse.Api.Common.Extensions;
using TalkPulse.Api.Common.Persistence;
;

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
//builder.AddNpgsqlDbContext<AppDbContext>("talkpulsedb");
// --> Will check it later to see what is the issue

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("default")));

// ── Messaging: RabbitMQ ───────────────────────────────────────────────────────
// "messaging" must match the resource name registered in AppHost.
builder.AddRabbitMQClient("messaging");

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// ── OpenAPI ───────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // seeding of data
    await SeedSpeakerDataAsync(db);
}

app.UseHttpsRedirection();

// Health check endpoints: /health and /alive
app.MapDefaultEndpoints();

var routeGroupBuilder = app.MapGroup("/api/v1").WithTags("TalkPulse API v1");
app.MapEndPoints(routeGroupBuilder);

app.Run();

static async Task SeedSpeakerDataAsync(AppDbContext db)
{
    Console.WriteLine("Seeding development data...");
    if (await db.Speakers.AnyAsync())
        return;

    var speaker = Speaker.Create("Sashikumar Yadav", "sky@gmail.com", "Qwerty@123");
    db.Speakers.Add(speaker);
    await db.SaveChangesAsync();

    Console.WriteLine("Seeded development data.");

}

