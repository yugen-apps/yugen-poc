using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Redis.Publisher.Workers;

public class ProducerWorker : BackgroundService
{
    private static readonly string ConnectionString = "localhost:6379";
    private static readonly ConnectionMultiplexer Connection =
        ConnectionMultiplexer.Connect(ConnectionString);
    private readonly ILogger<ProducerWorker> _logger;
    private const string Channel = "messages";

    public ProducerWorker(ILogger<ProducerWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        while (!stoppingToken.IsCancellationRequested)
        {
            // var message = new Message(Guid.NewGuid(), DateTime.UtcNow);

            // var json = JsonSerializer.Serialize(message);

            var message = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            await subscriber.PublishAsync(RedisChannel.Literal(Channel), new RedisValue(message));

            _logger.LogInformation("Sending message: {Channel} - {message}", Channel, message);

            await Task.Delay(5000, stoppingToken);
        }
    }
}

