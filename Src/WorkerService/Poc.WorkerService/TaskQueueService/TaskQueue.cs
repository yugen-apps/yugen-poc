using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Poc.WorkerService.TaskQueueService;

public class TaskQueue : ITaskQueue
{
    // https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service
    private readonly Channel<Func<CancellationToken, ValueTask>> _channel;

    public int Count => _channel.Reader.Count;

    public TaskQueue(int capacity)
    {
        BoundedChannelOptions options = new(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _channel = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    public async ValueTask QueueAsync(
            Func<CancellationToken, ValueTask> item)
    {
        ArgumentNullException.ThrowIfNull(item);

        await _channel.Writer.WriteAsync(item);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        Func<CancellationToken, ValueTask>? item =
            await _channel.Reader.ReadAsync(cancellationToken);

        return item;
    }
}
