namespace Poc.Redis.Common.Services;

public class AppMessage
{
    public AppMessage()
    {
    }

    public AppMessage(string redisId, string content, string sender)
    {
        RedisId = redisId;
        Content = content;
        Sender = sender;
    }

    public AppMessage(string content, string sender)
    {
        Content = content;
        Sender = sender;
    }

    public string? RedisId { get; init; }

    public string? Content { get; init; }

    public string? Sender { get; init; }
}