using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TalkPulse.Worker;

/// <summary>
/// Background service that consumes messages from the "talkpulse" RabbitMQ queue.
/// Inject additional services (e.g. DbContext, MediatR) here as needed.
/// </summary>
public sealed class MessageConsumerWorker : BackgroundService
{
    private readonly ILogger<MessageConsumerWorker> _logger;
    private readonly IConnection _connection;
    private const string QueueName = "talkpulse";

    public MessageConsumerWorker(ILogger<MessageConsumerWorker> logger, IConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MessageConsumerWorker starting. Queue: {Queue}", QueueName);

        using var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        // Fair dispatch — one unacked message per consumer at a time
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received message: {Message}", message);

            try
            {
                await ProcessMessageAsync(message, stoppingToken);
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message: {Message}", message);
                // Nack without requeue — send to dead-letter or discard
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("Listening for messages on '{Queue}'...", QueueName);

        // Block until cancellation is requested
        await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        _logger.LogInformation("MessageConsumerWorker stopping.");
    }

    private Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        // TODO: implement your message handling logic here
        // e.g., dispatch a MediatR command, persist to DB, call a domain service
        _logger.LogDebug("Processing: {Message}", message);
        return Task.CompletedTask;
    }
}
