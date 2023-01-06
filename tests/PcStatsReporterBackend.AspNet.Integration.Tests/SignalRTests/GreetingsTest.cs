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
    public async Task StartConnection_ShouldWork2()
    {
        var mainHubConnection = _mainHubConnection;

        await mainHubConnection.StartAsync();
    }
}