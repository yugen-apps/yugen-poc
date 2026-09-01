using Poc.Redis.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Redis.Subscriber.Services;

namespace Poc.Redis.Subscriber;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddRedisPubSub(builder.Configuration);
        builder.Services.AddSingleton<SubsriberService>();

        var host = builder.Build();

        var subsriberService = host.Services.GetRequiredService<SubsriberService>();
        subsriberService.Init();

        host.Run();
    }
}
