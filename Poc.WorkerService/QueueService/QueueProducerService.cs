using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.QueueService;

public class QueueProducerService : BackgroundService
{
    private readonly ILogger<QueueProducerService> _logger;
    private readonly IMessageQueue<string> _queue;

    public QueueProducerService(
        ILogger<QueueProducerService> logger,
        IMessageQueue<string> queue)
    {
        _logger = logger;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _queue.QueueAsync($"hello {DateTimeOffset.Now}", cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
    }
}
