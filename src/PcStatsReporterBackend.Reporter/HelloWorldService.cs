using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PcStatsReporterBackend.Reporter;

public class HelloWorldService : IHostedService
{
    private readonly ILogger<HelloWorldService> _logger;

    public HelloWorldService(ILogger<HelloWorldService> logger)
    {
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    _logger.LogInformation("Hello");
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