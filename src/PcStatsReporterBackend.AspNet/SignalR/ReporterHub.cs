using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using PcStatsReporterBackend.Contracts;
using PcStatsReporterBackend.Core;

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
        var coreAssembly = Assembly.GetAssembly(typeof(Sample));
        var allTypes = coreAssembly.GetTypes().ToList();
        
        Type? transportedType = coreAssembly.GetType(transportMessage.Type);

        var s = System.Text.Json.JsonSerializer.Serialize(transportMessage);
        _logger.LogInformation(s);

        var payload = System.Text.Json.JsonSerializer.Deserialize(transportMessage.Payload, transportedType);

        Sample sample = payload as Sample;

        if (sample is not null)
        {
            // ReceivedSamples++;
        }
        
        await Task.CompletedTask;
    }
}