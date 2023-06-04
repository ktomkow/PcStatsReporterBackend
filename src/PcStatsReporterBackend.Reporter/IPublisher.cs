namespace PcStatsReporterBackend.Reporter;

/// <summary>
/// Main signalr hub to talk to clients
/// </summary>
public interface IPublisher
{
    Task Publish(object obj);
}