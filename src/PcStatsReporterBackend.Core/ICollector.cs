namespace PcStatsReporterBackend.Core;

public interface ICollector<T> where T : Sample
{
    T Collect();
}