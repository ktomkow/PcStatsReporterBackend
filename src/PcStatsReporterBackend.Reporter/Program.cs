using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PcStatsReporterBackend.Reporter;

public class Program
{
    public static async Task<int> Main()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<HelloWorldService>();

                services.AddHttpClient();
            });
        
        await hostBuilder.RunConsoleAsync();

        return Environment.ExitCode;
    }
}