using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PcStatsReporterBackend.Reporter.Features.Hello;
using PcStatsReporterBackend.Reporter.Features.HelloNotificationsSubscriber;

namespace PcStatsReporterBackend.Reporter;

public class Program
{
    public static async Task<int> Main()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<HelloWorldService>();

                services.AddMediatR(Assembly.GetExecutingAssembly());
                services.AddTransient<HelloHandler>();
                services.AddTransient<HelloNotificationOne>();
                services.AddTransient<HelloNotificationTwo>();
                
                services.AddHttpClient();
            });
        
        await hostBuilder.RunConsoleAsync();

        return Environment.ExitCode;
    }
}