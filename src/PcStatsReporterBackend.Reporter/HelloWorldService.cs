using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PcStatsReporterBackend.Reporter.Features.Hello;
using PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;

namespace PcStatsReporterBackend.Reporter;

public class HelloWorldService : IHostedService
{
    private readonly ILogger<HelloWorldService> _logger;
    private readonly IMediator _mediator;

    public HelloWorldService(ILogger<HelloWorldService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var talkToServerRequest = new HelloRequest() { Hub = "reporter" };
                    var response = await _mediator.Send(talkToServerRequest, cancellationToken);
                    var helloNotification = new HelloNotification()
                    {
                        Message = response.Message
                    };

                    await _mediator.Publish(helloNotification, cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Application is shutting down..");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}