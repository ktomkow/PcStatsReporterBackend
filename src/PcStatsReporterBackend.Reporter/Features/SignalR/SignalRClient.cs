using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PcStatsReporterBackend.Contracts;
using PcStatsReporterBackend.Contracts.ToServer;
using PcStatsReporterBackend.Reporter.Features.SamplePublisher;

namespace PcStatsReporterBackend.Reporter.Features.SignalR;

public class SignalRClient
{
    private readonly ILogger<SignalRPublisher> _logger;
    private readonly SemaphoreSlim _mutex;
    private readonly HubConnection _hubConnection;

    public SignalRClient(ILogger<SignalRPublisher> logger, IHaveHubConnections hubConnections)
    {
        _logger = logger;
        _mutex = new SemaphoreSlim(1, 1);

        _hubConnection = hubConnections.GetReporterConnection();

        _hubConnection.Closed += async (Exception? e) =>
        {
            _logger.LogError("Hub connection closed! Error: {Error}", e);
            await Task.CompletedTask;
        };
    }

    private async Task Initialize(CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            _logger.LogInformation("Signal R initialization started");

            _hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(1);

            await _hubConnection.StartAsync(cancellationToken: cancellationToken);

            _logger.LogInformation("Signal R initialization finished");
        }
        catch (Exception e)
        {
            _logger.LogError("Error during SignalR initialization: {Error}", e);
            throw;
        }
    }

    public async Task Publish(string action, TransportMessage transportMessage, CancellationToken cancellationToken)
    {
        try
        {
            await _mutex.WaitAsync(cancellationToken);

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                _logger.LogInformation("Signal R not initialized");
                await Initialize(cancellationToken);
            }

            _logger.LogInformation("Sending message using SignalR");
            await _hubConnection.SendAsync(action, transportMessage, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Error during transfer message {MessageId}: {Error}",transportMessage.Id, e);
        }
        finally
        {
            _mutex.Release();
        }
    }
}