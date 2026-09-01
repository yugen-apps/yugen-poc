using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedisMessageLabApi.Workers;

public class ProducerWorker(ILogger<ProducerWorker> logger) : BackgroundService
{
    private static readonly string ConnectionString = "localhost:6379";
    private static readonly ConnectionMultiplexer Connection =
        ConnectionMultiplexer.Connect(ConnectionString);

    private const string Channel = "messages";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        while (!stoppingToken.IsCancellationRequested)
        {
            // var message = new Message(Guid.NewGuid(), DateTime.UtcNow);

            // var json = JsonSerializer.Serialize(message);

            var message = DateTime.UtcNow.ToString();

            await subscriber.PublishAsync(Channel, new RedisValue(message));

            logger.LogInformation("Sending message: {Channel} - {message}", Channel, message);

            await Task.Delay(5000, stoppingToken);
        }
    }
}

