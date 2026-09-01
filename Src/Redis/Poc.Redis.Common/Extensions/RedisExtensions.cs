using StackExchange.Redis;
using System;

namespace Poc.Redis.Common.Extensions;

public static class RedisExtensions
{
    public static T ConvertTo<T>(this RedisValue redisValue) where T : new()
    {
        if (redisValue.IsNull)
        {
            return new T();
        }

        return (T)Convert.ChangeType(redisValue, typeof(T));
    }
}