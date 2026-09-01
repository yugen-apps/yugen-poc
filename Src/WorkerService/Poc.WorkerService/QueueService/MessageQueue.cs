using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Poc.WorkerService.QueueService;


public class MessageQueue<T> : IMessageQueue<T> where T : class
{
    private readonly Channel<T> _channel;

    public MessageQueue(IOptions<BackgroundJobOptions> options)
    {
        _channel = Channel.CreateBounded<T>(options.Value.Capacity);

        //_channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity)
        //{
        //	FullMode = BoundedChannelFullMode.Wait,
        //	SingleReader = true,
        //	SingleWriter = false,
        //	AllowSynchronousContinuations = false
        //});
    }

    public ValueTask QueueAsync(T message, CancellationToken cancellationToken) =>
        _channel.Writer.WriteAsync(message, cancellationToken);

    public async ValueTask<T?> DequeueAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return default;
        }
        catch (ChannelClosedException)
        {
            return default;
        }
    }

    public int Count => _channel.Reader.Count;
}