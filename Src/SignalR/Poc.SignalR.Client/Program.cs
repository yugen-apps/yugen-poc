using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poc.SignalR.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.SignalR.Client;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<ConsumerWorker>();

        var host = builder.Build();
        host.Run();
    }
}

public class ConsumerWorker : BackgroundService
{
    private readonly ILogger<ConsumerWorker> _logger;
    private HubConnection _hubConnection;

    public ConsumerWorker(
        ILogger<ConsumerWorker> logger)
    {
        _logger = logger;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/ChatCLientHub")
            .Build();

        _hubConnection.On<string>(nameof(IChatClient.ReceiveMessage), OnReceiveMessage);

        _hubConnection.On("GetMessage", async () =>
        {
            Console.WriteLine("Enter message:");
            var message = await Console.In.ReadLineAsync();
            return message;
        });
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"{nameof(ConsumerWorker)} Started");

        //System.Console.WriteLine("Press any key");
        //System.Console.ReadLine();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _hubConnection.StartAsync(cancellationToken);
                System.Console.WriteLine("Connected");
                var message = await _hubConnection.InvokeAsync<string>("WaitForMessage", _hubConnection.ConnectionId);

                break;
            }
            catch
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }

    private void OnReceiveMessage(string message)
    {
        System.Console.WriteLine($"OnReceiveMessage: {message}");
    }
}
