using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.TaskQueueService;

public class TaskQueueProcessorService : BackgroundService
{
    private readonly ITaskQueue _taskQueue;
    private readonly ILogger<TaskQueueProcessorService> _logger;

    public TaskQueueProcessorService(
        ITaskQueue taskQueue,
        ILogger<TaskQueueProcessorService> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Name} is running...", nameof(TaskQueueProcessorService));

        return ProcessTaskQueueAsync(cancellationToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Func<CancellationToken, ValueTask>? item =
                    await _taskQueue.DequeueAsync(cancellationToken);

                await item(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(TaskQueueProcessorService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
