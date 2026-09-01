using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.QueueService;

public class QueueProcessorService : BackgroundService
{
    private readonly ILogger<QueueProcessorService> _logger;
    private readonly IMessageQueue<string> _queue;

    public QueueProcessorService(
        ILogger<QueueProcessorService> logger,
        IMessageQueue<string> queue)
    {
        _logger = logger;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Queue processor starting.");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await _queue.DequeueAsync(cancellationToken);
                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                await ProcessMessageAsync(message, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break; // expected during shutdown — exit the loop cleanly
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing queue message.");
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken); // brief backoff before retrying
            }
        }
        _logger.LogInformation("Queue processor stopping.");
    }

    private async Task ProcessMessageAsync(object message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ProcessMessageAsync: {message}", message);
    }
}
