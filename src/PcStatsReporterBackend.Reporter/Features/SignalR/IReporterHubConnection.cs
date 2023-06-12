using Microsoft.AspNetCore.SignalR.Client;

namespace PcStatsReporterBackend.Reporter.Features.SignalR;

public interface IHaveHubConnections
{
    public HubConnection GetReporterConnection();
}