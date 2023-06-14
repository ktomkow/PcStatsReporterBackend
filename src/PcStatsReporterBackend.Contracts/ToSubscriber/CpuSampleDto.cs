namespace PcStatsReporterBackend.Contracts.ToSubscriber
{
    public class CpuSampleDto : AbstractSampleDto
    {
        public uint Temperature { get; set; }
    }
}