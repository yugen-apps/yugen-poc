using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.TaskQueueService;

public class TaskQueueProducerService : BackgroundService
{
    private readonly ITaskQueue _taskQueue;
    private readonly ILogger<TaskQueueProducerService> _logger;

    public TaskQueueProducerService(
        ITaskQueue taskQueue,
        ILogger<TaskQueueProducerService> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _taskQueue.QueueAsync(QueueItemAsync);
            await Task.Delay(1000, cancellationToken);
        }
    }

    private async ValueTask QueueItemAsync(CancellationToken token)
    {
        _logger.LogInformation("Queued work item is starting...");

        while (!token.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }

            _logger.LogInformation("Queued work item  is running");
        }
    }
}