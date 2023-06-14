using System;

namespace PcStatsReporterBackend.Contracts.ToSubscriber
{
    public abstract class AbstractSampleDto
    {
        public Guid Id { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}