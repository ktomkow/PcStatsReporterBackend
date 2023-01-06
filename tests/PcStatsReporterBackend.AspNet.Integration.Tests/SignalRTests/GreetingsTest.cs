using Microsoft.AspNetCore.SignalR.Client;
using PcStatsReporterBackend.Contracts;

namespace PcStatsReporterBackend.AspNet.Integration.Tests.SignalRTests;

public class GreetingsTest : IntegrationTest
{
    public GreetingsTest(TestsFixture<Startup> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task StartConnection_ShouldWork()
    {
        var mainHubConnection = _mainHubConnection;

        await mainHubConnection.StartAsync();
    }
    
    [Fact]
    public async Task StartConnection_WhenConnectionStarted_GreetingShouldBeExecutedOnClient()
    {
        var mainHubConnection = _mainHubConnection;
        ServerWelcome response = new ();
        
        mainHubConnection.On<ServerWelcome>("greeting", (serverWelcome) =>
        {
            response = serverWelcome;
        });

        await mainHubConnection.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(1));

        response.Should().NotBeNull();
        response.Message.Should().Be("Hello");
    }
}