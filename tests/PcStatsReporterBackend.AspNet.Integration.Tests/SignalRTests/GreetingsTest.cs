using Microsoft.AspNetCore.SignalR.Client;

namespace PcStatsReporterBackend.AspNet.Integration.Tests.SignalRTests;

public class GreetingsTest : IntegrationTest
{
    public GreetingsTest(TestsFixture<Startup> fixture) : base(fixture)
    {

    }
    
    [Fact]
    public async Task Test()
    {
        var hubUri = "main";

        Uri baseAddress = _fixture.Server.BaseAddress;
        var address = baseAddress + hubUri;
        
        // https://lurumad.github.io/integration-tests-in-aspnet-core-signalr
        await using var connection = new HubConnectionBuilder()
            .WithUrl(address, o => o.HttpMessageHandlerFactory = _ => _fixture.Server.CreateHandler())
            .Build();

        await connection.StartAsync();
    }
}