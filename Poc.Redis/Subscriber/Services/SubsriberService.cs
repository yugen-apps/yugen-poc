using Common.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace Subscriber.Services;

public class SubsriberService
{
    private readonly IRedisService _redisService;
    private readonly ILogger<SubsriberService> _logger;

    public SubsriberService(
        IRedisService redisService,
        ILogger<SubsriberService> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    public void Init()
    {
        _redisService.Subscribe(OnMessage);
    }

    private void OnMessage(RedisChannel channel, RedisValue value)
    {
        try
        {
            var message = JsonSerializer.Deserialize<AppMessage>(value.ToString());

            _logger.LogInformation($"Message received from {channel} " +
                $"Id: {message.RedisId} " +
                $"Content: {message.Content} " +
                $"Sender: {message.Sender}");
        }
        catch (Exception ex)
        {
            _logger.LogError("OnMessage exception: {Message}", ex.Message);
        }
    }

    private void Add()
    {

    }

    private void Get()
    {

    }
}
