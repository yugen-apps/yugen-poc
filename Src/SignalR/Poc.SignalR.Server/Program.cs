using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poc.SignalR.Server.Hubs;
using Poc.SignalR.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.SignalR.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Server
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<ProducerWorker>();

        var app = builder.Build();

        // Server
        app.MapHub<ChatCLientHub>("/ChatCLientHub");

        app.Run();
    }
}

public class ProducerWorker : BackgroundService
{
    private readonly IHubContext<ChatCLientHub, IChatClient> _hubContext;
    private readonly ILogger<ProducerWorker> _logger;

    public ProducerWorker(
        IHubContext<ChatCLientHub, IChatClient> hubContext,
        ILogger<ProducerWorker> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"{nameof(ProducerWorker)} Started");

        var group = _hubContext.Clients.Group("SignalR Users");

        while (!ChatCLientHub.IsConnected)
        {
            await Task.Delay(100);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            System.Console.WriteLine("Message:");
            var message = System.Console.ReadLine() ?? string.Empty;

            await _hubContext.Clients.All.ReceiveMessage(message);
        }
    }
}