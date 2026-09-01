using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Redis.Common.Services;

public class RedisService : IRedisService
{
    private const string ChannelName = "live_updates";
    private const string StreamKey = "audit_stream";

    private readonly ILogger<RedisService> _logger;
    private readonly IDatabase _db;
    private readonly ISubscriber _subscriber;

    private long _producedTotal;

    public RedisService(
        IConnectionMultiplexer multiplexer,
        ILogger<RedisService> logger)
    {
        _logger = logger;

        _db = multiplexer.GetDatabase();
        _subscriber = multiplexer.GetSubscriber();
    }

    public async Task<long> PublishAsync(AppMessage message)
    {
        string json = JsonSerializer.Serialize(message);

        // Publish returns the count of active subscribers
        long subscribersCount = await _subscriber.PublishAsync(RedisChannel.Literal(ChannelName), json);

        if (subscribersCount != 0)
        {
            return subscribersCount;
        }

        _logger.LogInformation("[PUB/SUB WARNING] Message {message} was sent, but 0 subscribers were listening. Data is lost!", message.RedisId);

        return subscribersCount;
    }

    public void Subscribe(Action<RedisChannel, RedisValue> handler)
    {
        _subscriber.Subscribe(RedisChannel.Literal(ChannelName), handler);
    }

    public string Produce(AppMessage appMessage)
    {
        return ProduceBatch([appMessage])[0];
    }

    /// <summary>
    /// Pipeline several XADD calls in one round trip.
    /// Each entry carries an approximate MAXLEN cap. The "~" flavour
    /// lets Redis trim at a macro-node boundary, which is much cheaper
    /// than exact trimming and is the right call for a retention
    /// guardrail rather than a hard size limit.
    /// </summary>
    private string[] ProduceBatch(IEnumerable<AppMessage?> payload)
    {
        var payloadList = payload.ToList();
        if (payloadList.Count == 0)
        {
            return Array.Empty<string>();
        }

        var batch = _db.CreateBatch();
        var addTasks = new Task<RedisValue>[payloadList.Count];
        for (var i = 0; i < payloadList.Count; i++)
        {
            var pairs = MessageToNameValueEntries(payloadList[i]);
            addTasks[i] = batch.StreamAddAsync(
                StreamKey,
                pairs);
        }
        batch.Execute();
        Task.WaitAll(addTasks);

        var ids = new string[addTasks.Length];
        for (var i = 0; i < addTasks.Length; i++)
        {
            ids[i] = (string)addTasks[i].Result!;
        }
        Interlocked.Add(ref _producedTotal, ids.Length);
        return ids;
    }

    public async Task<IEnumerable<AppMessage>?> ConsumeAsync()
    {
        var messages = await _db.StreamReadAsync(StreamKey, "0-0", count: 10);
        if (messages.Length == 0)
        {
            return null;
        }

        var result = messages.Select(x => new AppMessage(x.Id.ToString(), x.Values[1].Value!, x.Values[2].Value!));
        return result;
    }

    //public async Task AppendAsync(AppMessage message)
    //{
    //    await _db.StreamAddAsync(StreamKey, MessageToNameValueEntries(message));
    //}

    private static NameValueEntry[] MessageToNameValueEntries(AppMessage message) => [
        new NameValueEntry("Id", message.RedisId),
        new NameValueEntry("content", message.Content),
        new NameValueEntry("Sender", message.Sender)
    ];


    public async Task<RedisValue[]> ConsumeAndStackDeleteAsync()
    {
        StreamEntry[] messages = await _db.StreamReadAsync(StreamKey, "0-0", count: 1);
        var redisValues = messages.Select(x => x.Id).ToArray();
        await _db.StreamDeleteAsync(StreamKey, redisValues);
        return redisValues;
    }

    public async Task<List<AppMessage>> PeekHistoryAsync()
    {
        // XRANGE audit_stream - + (Read everything from start to finish)
        var entries = await _db.StreamRangeAsync(StreamKey, "-", "+");

        return entries.Select(e => new AppMessage(
            e.Values.FirstOrDefault(v => v.Name == "id").Value!,
            e.Values.FirstOrDefault(v => v.Name == "content").Value!,
            "Stream History"
        )).ToList();
    }

}