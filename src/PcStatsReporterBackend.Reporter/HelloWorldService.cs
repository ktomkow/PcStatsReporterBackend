using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PcStatsReporterBackend.Contracts;

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
                    await TalkToServer(cancellationToken);
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

    private async Task TalkToServer(CancellationToken cancellationToken)
    {
        await using var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:7000/main")
            .Build();
        
        hubConnection.On<ServerWelcome>("greeting", (serverWelcome) =>
        {
            _logger.LogInformation(serverWelcome.Message);
        });

        _logger.LogInformation("Lets start connection");
        await hubConnection.StartAsync(cancellationToken);

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        
        _logger.LogInformation("Ok, let's finish talk");
        await hubConnection.StopAsync(cancellationToken);
    }
}