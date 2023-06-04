using Microsoft.AspNetCore.SignalR;
using PcStatsReporterBackend.Contracts;

namespace PcStatsReporterBackend.AspNet.SignalR;

/// <summary>
/// Hub that collects data
/// </summary>
public class ReporterHub : Hub
{
    private readonly ILogger<ReporterHub> _logger;

    public ReporterHub(ILogger<ReporterHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        ServerWelcome serverWelcome = new()
        {
            Message = "Hello"
        };

        var userIdentifier = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;
        
        
        _logger.LogInformation("User identifier: {Identifier}, connection identifier: {Connection}", userIdentifier, connectionId);
        await Clients.Caller.SendCoreAsync("greeting", new object?[]{serverWelcome});
    }
}