using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.LibreHardware;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.Reporter;

public class Program
{
    public static async Task<int> Main()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                // services.AddHostedService<HelloWorldService>();
                services.AddHostedService<CollectorService>();

                services.AddMediatR(Assembly.GetExecutingAssembly());
                // services.AddTransient<HelloHandler>();
                // services.AddTransient<HelloNotificationOne>();
                // services.AddTransient<HelloNotificationTwo>();
                services.AddSingleton<SignalRClient>();
                services.AddSingleton<ICollector<CpuSample>, CpuCollector>();
                
                services.AddHttpClient();
            });
        
        await hostBuilder.RunConsoleAsync();

        return Environment.ExitCode;
    }
}