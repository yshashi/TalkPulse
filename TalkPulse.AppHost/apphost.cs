#:sdk Aspire.AppHost.Sdk@13.2.0
#:package Aspire.Hosting.PostgreSQL@13.2.0
#:package Aspire.Hosting.RabbitMQ@13.2.0
#:project ../TalkPulse.Api/TalkPulse.Api.csproj
#:project ../TalkPulse.Worker/TalkPulse.Worker.csproj

var builder = DistributedApplication.CreateBuilder(args);

// ── Database: PostgreSQL ──────────────────────────────────────────────────────
// Runs a Postgres container. WithPgAdmin adds a browser-based admin UI.
// WithDataVolume persists data across container restarts.
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume(); //check more about this in the documentation: https://docs.aspire.dev/apphost/features/databases#data-persistence

var db = postgres.AddDatabase("talkpulsedb");

// ── Messaging: RabbitMQ ───────────────────────────────────────────────────────
// WithManagementPlugin adds the RabbitMQ Management UI on port 15672.
var messaging = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

// ── API ───────────────────────────────────────────────────────────────────────
builder.AddProject<Projects.TalkPulse_Api>("api")
    .WithReference(db)
    .WithReference(messaging)
    .WaitFor(db)
    .WaitFor(messaging);

// ── Worker (background message consumer) ─────────────────────────────────────
builder.AddProject<Projects.TalkPulse_Worker>("worker")
    .WithReference(messaging)
    .WaitFor(messaging);

builder.Build().Run();

