using MediatR;
using PcStatsReporterBackend.Core;

namespace PcStatsReporterBackend.Reporter.Features.SamplePublisher;

public class SampleNotification : INotification
{
    public Sample Sample { get; set; }
}