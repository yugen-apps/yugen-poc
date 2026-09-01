using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Redis.Common.Extensions;
using Poc.Redis.Subscriber.Services;

namespace Poc.Redis.Subscriber;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddRedisPubSub(builder.Configuration);
        builder.Services.AddSingleton<SubsriberService>();

        //var multiplexer = ConnectionMultiplexer.Connect("localhost:6379");
        //builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        //builder.Services.AddScoped<IRedisService, RedisService>();
        //builder.Services.AddHostedService<ProducerWorker>();
        //builder.Services.AddHostedService<ConsumerWorker>();

        var host = builder.Build();

        var subsriberService = host.Services.GetRequiredService<SubsriberService>();
        subsriberService.Init();

        //// Endpoint 1: Test Pub/Sub
        //app.MapPost("/broadcast", async (AppMessage message, IRedisService service) =>
        //{
        //    long listeners = await service.PublishAsync(message);
        //    return Results.Ok(new
        //    {
        //        Mechanism = "Pub/Sub",
        //        ActiveListeners = listeners,
        //        Status = listeners > 0 ? "Delivered" : "Lost (No Listeners)"
        //    });
        //});

        //// Endpoint 2: Test Stream Storage
        //app.MapPost("/log", async (AppMessage message, IRedisService service) =>
        //{
        //    await service.AppendAsync(message);
        //    return Results.Ok("Message logged in Stream.");
        //});

        //// Endpoint 3: Test Stream Consume & Delete
        //app.MapPost("/log/process", async (IRedisService service) =>
        //{
        //    var note = await service.ConsumeAndStackDeleteAsync();
        //    return note is null ? Results.NotFound("No logs left.") : Results.Ok(note);
        //});

        //// Endpoint 4: Peek at History (Without deleting)
        //app.MapGet("/log/history", async (IRedisService service) =>
        //{
        //    var history = await service.PeekHistoryAsync();
        //    return history.Count != 0 ? Results.Ok(history) : Results.NotFound("The Stream is empty.");
        //});

        host.Run();
    }
}
