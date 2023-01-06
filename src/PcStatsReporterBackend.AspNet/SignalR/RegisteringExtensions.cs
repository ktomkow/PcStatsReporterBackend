namespace PcStatsReporterBackend.AspNet.SignalR;

public static class RegisteringExtensions
{
    public static void AddMySignalR(this IServiceCollection services)
    {
        services.AddSignalR();
    }

    public static void MapMySignalR(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<MainHub>("/main");
    }
}