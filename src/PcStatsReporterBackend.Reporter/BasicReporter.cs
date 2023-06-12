using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.LibreHardware;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.Reporter;

public class BasicReporter
{
    public static async Task<int> Main()
    {
        var reporterServices = new ReporterServices();

        var hostBuilder = CreateHostBuilder(reporterServices);
        
        await hostBuilder.RunConsoleAsync();

        return Environment.ExitCode;
    }

    public static IHostBuilder CreateHostBuilder(ReporterServices reporterServices)
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                reporterServices.RegisterApp(services);
                reporterServices.RegisterHubs(services);
            });

        return hostBuilder;
    }
}