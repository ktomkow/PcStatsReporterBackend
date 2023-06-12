using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PcStatsReporterBackend.Contracts;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.Reporter.Features.SamplePublisher;

public class SignalRPublisher : INotificationHandler<SampleNotification>
{
    private readonly ILogger<SignalRPublisher> _logger;
    private readonly SignalRClient _signalRClient;

    public SignalRPublisher(ILogger<SignalRPublisher> logger, SignalRClient signalRClient)
    {
        _logger = logger;
        _signalRClient = signalRClient;
    }
    
    public async Task Handle(SampleNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("SignalR - handle sample with notification");
            
            var transportMessage = new TransportMessage()
            {
                Type = notification.Sample.GetType().FullName,
                Payload = JsonConvert.SerializeObject(notification.Sample)
            };

            await _signalRClient.Publish("transferSample", transportMessage, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Error during sending sample using SignalR", e);
        }
    }
}