using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using PcStatsReporterBackend.Contracts;
using PcStatsReporterBackend.Contracts.ToServer;
using PcStatsReporterBackend.Contracts.ToSubscriber;
using PcStatsReporterBackend.Core;

namespace PcStatsReporterBackend.AspNet.SignalR;

/// <summary>
/// Hub that collects data
/// </summary>
public class ReporterHub : Hub
{
    public const string OnCpuSample = "OnCpuSample";
    public const string OnSampleReceived = "OnSampleReceived";
    public const string OnSubscription = "OnSubscription";

    public const string SubscribeFunction = nameof(ReporterHub.Subscribe);
    
    private const string SubscriberGroup = "SubscriberGroup";
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

        CpuSample sample = payload as CpuSample;

        if (sample is not null)
        {
            _logger.LogInformation("Temperature: {Temperature}", sample.Temperature);

            await Clients.Caller.SendAsync(OnSampleReceived,
                new SampleConfirmation() { MessageId = transportMessage.Id});
            
            await Clients.Group(SubscriberGroup).SendAsync(ReporterHub.OnCpuSample, new CpuSampleDto()
            {
                Id = sample.Id,
                Temperature = sample.Temperature,
                RegisteredAt = sample.RegisteredAt
            });
        }
        else
        {
            await Clients.Caller.SendAsync(OnSampleReceived,
                new SampleConfirmation() { MessageId = transportMessage.Id, Error = "Not deserialized"});
        }
    }

    public async Task Subscribe()
    {
        var connectionId = Context.ConnectionId;

        try
        {
            _logger.LogInformation("Adding connection {ConnectionId} to subscribers group", connectionId);

            await Groups.AddToGroupAsync(connectionId, SubscriberGroup);
            await Clients.Caller.SendAsync(OnSubscription,
                new SubscriptionConfirmation() { IsSuccess = true });

        }
        catch (Exception e)
        {
            _logger.LogError("Adding connection {ConnectionId} to subscribers group failed: {Error}", connectionId, e);
            await Clients.Caller.SendAsync(OnSubscription,
                new SubscriptionConfirmation() { IsSuccess = false });
        }
    }
}