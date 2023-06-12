namespace PcStatsReporterBackend.Core;

public abstract class Sample
{
    public Guid Id { get; set; }
    public DateTime RegisteredAt { get; set; }
}