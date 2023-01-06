using MediatR;
using Microsoft.Extensions.Logging;

namespace PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;

public class HelloNotificationOne :  INotificationHandler<HelloNotification>
{
    private readonly ILogger<HelloNotificationOne> _logger;

    public HelloNotificationOne(ILogger<HelloNotificationOne> logger)
    {
        _logger = logger;
    }

    public Task Handle(HelloNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {Handler} got message: {Message}", nameof(HelloNotificationOne), notification.Message);
        return Task.CompletedTask;
    }
}