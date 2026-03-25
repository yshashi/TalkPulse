using Serilog;
using Serilog.Sinks.OpenTelemetry;
using TalkPulse.Worker;

var builder = Host.CreateApplicationBuilder(args);

// ── Serilog ──────────────────────────────────────────────────────────────────
// Writes structured logs to Console and to the Aspire dashboard via OTLP.
var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .WriteTo.OpenTelemetry(options =>
    {
        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            // gRPC endpoint — Aspire dashboard listens on the base OTLP URL
            options.Endpoint = otlpEndpoint;
            options.Protocol = OtlpProtocol.Grpc;

            var apiKey = builder.Configuration["ASPIRE_DASHBOARD_OTLP_PRIMARY_API_KEY"];
            if (!string.IsNullOrWhiteSpace(apiKey))
                options.Headers = new Dictionary<string, string> { ["x-otlp-api-key"] = apiKey };
        }

        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "talkpulse-worker",
            ["service.version"] = "1.0.0"
        };
    })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger, dispose: true);

// ── Aspire service defaults (OpenTelemetry metrics + tracing, health checks) ─
builder.AddServiceDefaults();

// ── Messaging: RabbitMQ ───────────────────────────────────────────────────────
// Connection name must match the AppHost reference name ("messaging")
builder.AddRabbitMQClient("messaging");

// ── Background worker ─────────────────────────────────────────────────────────
builder.Services.AddHostedService<MessageConsumerWorker>();

var host = builder.Build();
host.Run();
