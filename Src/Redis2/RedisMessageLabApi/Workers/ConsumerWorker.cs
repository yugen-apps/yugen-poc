using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace RedisMessageLabApi.Workers;

public class ConsumerWorker(ILogger<ConsumerWorker> logger) : BackgroundService
{
    private static readonly string ConnectionString = "localhost:6379";
    private static readonly ConnectionMultiplexer Connection =
        ConnectionMultiplexer.Connect(ConnectionString);

    private const string Channel = "messages";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        await subscriber.SubscribeAsync(Channel, (channel, message) =>
        {
            // var message = JsonSerializer.Deserialize<Message>(message);

            logger.LogInformation("Received message: {channel} - {message}", channel, message);
        }
        );
    }
}

