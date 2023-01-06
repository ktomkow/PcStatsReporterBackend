using MediatR;

namespace PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;

public class HelloNotification : INotification
{
    public string Message { get; set; }
}