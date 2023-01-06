using MediatR;
using Microsoft.Extensions.Logging;

namespace PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;

public class HelloNotificationTwo :  INotificationHandler<HelloNotification>
{
    private readonly ILogger<HelloNotificationTwo> _logger;

    public HelloNotificationTwo(ILogger<HelloNotificationTwo> logger)
    {
        _logger = logger;
    }

    public Task Handle(HelloNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {Handler} got message: {Message}", nameof(HelloNotificationTwo), notification.Message);
        return Task.CompletedTask;
    }
}