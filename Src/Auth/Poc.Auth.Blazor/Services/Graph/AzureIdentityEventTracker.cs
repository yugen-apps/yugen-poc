using Azure.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.Tracing;

namespace Poc.Auth.Blazor.Services.Graph;

public sealed class AzureIdentityEventTracker : IDisposable
{
    private readonly ILogger<AzureIdentityEventTracker> _logger;
    private readonly AzureEventSourceListener _listener;

    public AzureIdentityEventTracker(ILogger<AzureIdentityEventTracker> logger)
    {
        _logger = logger;
        _listener = new AzureEventSourceListener(HandleEvent, EventLevel.Informational);

        _logger.LogInformation("Hello World");
    }

    private void HandleEvent(EventWrittenEventArgs args, string message)
    {
        if (args is { EventSource.Name: "Azure-Identity" })
        {
            _logger.LogInformation("AzureIdentityEventTracker: {EventName} {message}", args.EventName, message);
        }
    }

    public void Dispose() => _listener?.Dispose();
}