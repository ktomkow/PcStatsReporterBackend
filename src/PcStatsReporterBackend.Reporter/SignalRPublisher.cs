using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace PcStatsReporterBackend.Reporter;

public class SignalRPublisher : IPublisher
{
    private readonly ILogger<SignalRPublisher> _logger;
    private readonly SemaphoreSlim _mutex;
    private HubConnection _hubConnection;

    public SignalRPublisher(ILogger<SignalRPublisher> logger)
    {
        _logger = logger;
        _mutex = new SemaphoreSlim(0, 1);
    }

    public async Task Initialize()
    {
        await _mutex.WaitAsync();
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:7000/reporter")
            .Build();

        _mutex.Release();
    }
    
    public async Task Publish(object obj)
    {
        await _mutex.WaitAsync();

        _mutex.Release();
    }
}