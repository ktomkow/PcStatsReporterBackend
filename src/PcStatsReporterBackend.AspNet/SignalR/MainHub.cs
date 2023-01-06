using Microsoft.AspNetCore.SignalR;
using PcStatsReporterBackend.Contracts;

namespace PcStatsReporterBackend.AspNet.SignalR;

/// <summary>
/// Main signalr hub to talk to clients
/// </summary>
public class MainHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        ServerWelcome serverWelcome = new()
        {
            Message = "Hello"
        };

        await Clients.Caller.SendCoreAsync("greeting", new []{serverWelcome});
    }
}