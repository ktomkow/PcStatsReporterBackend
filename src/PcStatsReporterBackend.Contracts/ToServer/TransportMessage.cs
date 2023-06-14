using System;

namespace PcStatsReporterBackend.Contracts.ToServer
{
    public class TransportMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}