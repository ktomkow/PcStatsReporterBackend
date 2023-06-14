using System;

namespace PcStatsReporterBackend.Contracts
{
    public class SampleConfirmation
    {
        public Guid MessageId { get; set; }
        public string? Error { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);
    }
}