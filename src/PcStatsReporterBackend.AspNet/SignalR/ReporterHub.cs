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
    
    /// <summary>
    /// Function to transfer samples
    /// </summary>
    public async Task TransferSample(TransportMessage transportMessage)
    {
        var s = System.Text.Json.JsonSerializer.Serialize(transportMessage);
        _logger.LogInformation(s);
        
        // System.Text.Json.JsonSerializer.Deserialize</**/>()

        await Task.CompletedTask;
    }
}