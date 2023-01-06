using Microsoft.AspNetCore.Mvc.Testing;
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
        var uri = "/main";

        await using var connection = new HubConnectionBuilder().WithUrl(_fixture.Server.BaseAddress + "/main").Build();

        // await connection.StartAsync();
    }
}