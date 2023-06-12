using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.LibreHardware;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.Reporter;

public class ReporterServices
{
    public virtual void RegisterHubs(IServiceCollection services)
    {
        
    }

    public void RegisterApp(IServiceCollection services)
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
    }
}