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
        var mainHubConnection = GetMainHubConnection();

        await mainHubConnection.StartAsync();
    }
    
    [Fact]
    public async Task StartConnection_WhenConnectionStarted_GreetingShouldBeExecutedOnClient()
    {
        var mainHubConnection = GetMainHubConnection();
        ServerWelcome response = new ();
        
        mainHubConnection.On<ServerWelcome>("greeting", (serverWelcome) =>
        {
            response = serverWelcome;
            _delayTokenSource.Cancel();
        });

        await mainHubConnection.StartAsync();

        await Delay(30);

        response.Should().NotBeNull();
        response.Message.Should().Be("Hello");
    }
}