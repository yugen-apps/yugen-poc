using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisMessageLabApi.Interfaces;

public interface IRedisService
{
    Task<long> PublishAsync(AppMessage message);
    Task AppendAsync(AppMessage message);
    Task<AppMessage?> ConsumeAndStackDeleteAsync();
    Task<List<AppMessage>> PeekHistoryAsync();
}