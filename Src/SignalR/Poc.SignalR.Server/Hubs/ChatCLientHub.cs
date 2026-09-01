using Microsoft.AspNetCore.SignalR;
using Poc.SignalR.Shared;
using System;
using System.Threading.Tasks;

namespace Poc.SignalR.Server.Hubs;

public class ChatCLientHub : Hub<IChatClient>
{
    public static bool IsConnected;

    public static string GroupName = "SignalR Users";

    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveMessage(message);
    }

    public async Task<string> WaitForMessage(string connectionId)
    {
        string message = await Clients.Client(connectionId).GetMessage();
        return message;
    }

    public override async Task OnConnectedAsync()
    {
        IsConnected = true;
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        IsConnected = false;
        await base.OnDisconnectedAsync(exception);
    }
}
