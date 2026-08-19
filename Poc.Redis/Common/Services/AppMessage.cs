namespace Common.Services;

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

    public string RedisId { get; set;  }

    public string Content { get; set; }

    public string Sender { get; set; }
}