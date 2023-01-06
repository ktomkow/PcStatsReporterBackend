using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PcStatsReporterBackend.Contracts;

namespace PcStatsReporterBackend.Reporter.Features.Hello;

public class HelloHandler : IRequestHandler<HelloRequest, HelloResponse>
{
    private readonly ILogger<HelloHandler> _logger;

    public HelloHandler(ILogger<HelloHandler> logger)
    {
        _logger = logger;
    }

    public async Task<HelloResponse> Handle(HelloRequest request, CancellationToken cancellationToken)
    {
        await using var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:7000/" + request.Hub)
            .Build();

        HelloResponse response = new HelloResponse();
        
        hubConnection.On<ServerWelcome>("greeting", (serverWelcome) =>
        {
            response.Message = serverWelcome.Message;
        });

        _logger.LogInformation("Lets start connection");
        await hubConnection.StartAsync(cancellationToken);

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        
        _logger.LogInformation("Ok, let's finish talk");
        await hubConnection.StopAsync(cancellationToken);

        return response;
    }
}