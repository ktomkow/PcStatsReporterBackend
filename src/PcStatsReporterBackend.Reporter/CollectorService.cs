using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.Reporter.Features.Hello;
using PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;
using PcStatsReporterBackend.Reporter.Features.SamplePublisher;

namespace PcStatsReporterBackend.Reporter;

public class CollectorService : IHostedService
{
    private readonly ILogger<CollectorService> _logger;
    private readonly IMediator _mediator;
    private readonly ICollector<CpuSample> _cpuSampleCollector;

    public CollectorService(ILogger<CollectorService> logger, IMediator mediator, ICollector<CpuSample> cpuSampleCollector)
    {
        _logger = logger;
        _mediator = mediator;
        _cpuSampleCollector = cpuSampleCollector;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    _logger.LogInformation("Get sample");
                    var sample = _cpuSampleCollector.Collect();

                    var sampleNotification = new SampleNotification()
                    {
                        Sample = sample
                    };
                        
                    await _mediator.Publish(sampleNotification, cancellationToken);
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