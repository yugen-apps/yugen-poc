using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poc.Redis.Common.Services;

public interface IRedisService
{
    Task<long> PublishAsync(AppMessage message);
    void Subscribe(Action<RedisChannel, RedisValue> handler);

    string Produce(AppMessage appMessage);
    Task<IEnumerable<AppMessage>?> ConsumeAsync();

    //Task AppendAsync(AppMessage message);
    //Task<AppMessage?> ConsumeAndStackDeleteAsync();
    //Task<List<AppMessage>> PeekHistoryAsync();
}