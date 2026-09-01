using Poc.Redis.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace Poc.Redis.Common.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRedisPubSub(this IServiceCollection services, IConfiguration configuration)
    {
        if (GetRedisConnectionString(configuration) is not string redisConnectionString)
        {
            throw new ArgumentException("The app hasn't been configured for Redis yet.");
        }

        var connection = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(connection);
        services.AddScoped<IRedisService, RedisService>();
        return services;
    }

    private static string? GetRedisConnectionString(IConfiguration configuration)
    {
        var azureContainerAppsScenario = new AzureContainerAppsServiceConnectorScenario();
        var localhostScenario = new LocalhostAppSettingsScenario();
        azureContainerAppsScenario.SetNext(localhostScenario);
        return azureContainerAppsScenario.BuildRedisConnectionString(configuration);
    }
}
