using Microsoft.AspNetCore.SignalR;

namespace PcStatsReporterBackend.AspNet.SignalR;

/// <summary>
/// Main signalr hub to talk to clients
/// </summary>
public class MainHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}