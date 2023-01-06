using MediatR;

namespace PcStatsReporterBackend.Reporter.Features.Hello;

public class HelloRequest : IRequest<HelloResponse>
{
    public string Hub { get; set; }
}