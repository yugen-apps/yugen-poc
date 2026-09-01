using System.Threading;
using System.Threading.Tasks;

namespace Poc.WorkerService.QueueService;

public interface IMessageQueue<T> where T : class
{
    int Count { get; }

    ValueTask QueueAsync(T message, CancellationToken cancellationToken);

    ValueTask<T?> DequeueAsync(CancellationToken cancellationToken);
}