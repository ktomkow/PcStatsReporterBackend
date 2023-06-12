using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace PcStatsReporterBackend.AspNet.Integration.Tests;

public class TestsFixture<TStartup> : WebApplicationFactory<Startup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(
                services =>
                {

                }
            )
            .UseEnvironment("Production")
            .UseStartup<Startup>();
    }
}