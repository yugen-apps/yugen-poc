using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Redis.Subscriber.Workers;

public class ConsumerWorker : BackgroundService
{
    private static readonly string ConnectionString = "localhost:6379";
    private static readonly ConnectionMultiplexer Connection =
        ConnectionMultiplexer.Connect(ConnectionString);
    private readonly ILogger<ConsumerWorker> _logger;
    private const string Channel = "messages";

    public ConsumerWorker(ILogger<ConsumerWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        await subscriber.SubscribeAsync(RedisChannel.Literal(Channel), (channel, message) =>
        {
            // var message = JsonSerializer.Deserialize<Message>(message);

            _logger.LogInformation("Received message: {channel} - {message}", channel, message);
        }
        );
    }
}

