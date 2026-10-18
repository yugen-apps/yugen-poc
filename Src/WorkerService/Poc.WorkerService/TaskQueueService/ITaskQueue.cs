using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.TaskQueueService;

public interface ITaskQueue
{
    int Count { get; }

    ValueTask QueueAsync(Func<CancellationToken, ValueTask> item);

    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
