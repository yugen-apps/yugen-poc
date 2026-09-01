using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisMessageLabApi.Services;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private const string Channel = "live_updates";
    private const string Stream = "audit_stream";

    public async Task<long> PublishAsync(AppMessage message)
    {
        var sub = redis.GetSubscriber();
        string json = JsonSerializer.Serialize(message);

        // Publish returns the count of active subscribers
        long subscribersCount = await sub.PublishAsync(RedisChannel.Literal(Channel), json);

        if (subscribersCount != 0) return subscribersCount;

        // This will show up in your .NET Terminal/Console
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            $"[PUB/SUB WARNING] Message {message.Id} was sent, but 0 subscribers were listening. Data is lost!");
        Console.ResetColor();

        return subscribersCount;
    }

    public async Task AppendAsync(AppMessage message)
    {
        var db = redis.GetDatabase();
        await db.StreamAddAsync(Stream, [
            new NameValueEntry("id", message.Id),
            new NameValueEntry("content", message.Content)
        ]);
    }

    public async Task<AppMessage?> ConsumeAndStackDeleteAsync()
    {
        var db = redis.GetDatabase();
        // Read the oldest message
        var messages = await db.StreamReadAsync(Stream, "0-0", count: 1);
        if (messages.Length == 0) return null;

        var msg = messages.First();
        var note = new AppMessage(msg.Values[0].Value!, msg.Values[1].Value!, "Stream");

        // ANSWERING YOUR DOUBT: Delete after consuming
        await db.StreamDeleteAsync(Stream, [msg.Id]);
        return note;
    }

    public async Task<List<AppMessage>> PeekHistoryAsync()
    {
        var db = redis.GetDatabase();

        // XRANGE audit_stream - + (Read everything from start to finish)
        var entries = await db.StreamRangeAsync(Stream, "-", "+");

        return entries.Select(e => new AppMessage(
            e.Values.FirstOrDefault(v => v.Name == "id").Value!,
            e.Values.FirstOrDefault(v => v.Name == "content").Value!,
            "Stream History"
        )).ToList();
    }
}