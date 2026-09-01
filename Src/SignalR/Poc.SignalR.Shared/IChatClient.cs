namespace Poc.SignalR.Shared;

public interface IChatClient
{
    Task ReceiveMessage(string message);

    Task<string> GetMessage();
}
