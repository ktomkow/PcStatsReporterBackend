using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PcStatsReporterBackend.AspNet.SignalR;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.Reporter;
using PcStatsReporterBackend.Reporter.Features.SamplePublisher;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.AspNet.Integration.Tests.SignalRTests;

public class SignalRPublisherTests: IntegrationTest
{
    public SignalRPublisherTests(TestsFixture<Startup> fixture) : base(fixture)
    {
        // _fixture.
    }

    [Fact]
    public async Task Start_Yeah_Yeah()
    {
        var testServices = new TestServices(GetReporterHubConnection());
        var hostBuilder = BasicReporter.CreateHostBuilder(testServices);
        
        var host = hostBuilder.Build();

        var reporterMediator = host.Services.GetRequiredService<IMediator>();

        var sampleNotification = new SampleNotification()
        {
            Sample = new CpuSample()
            {
                Id = Guid.Parse("00203459-cfd9-4d28-a83c-6b5dc5c6e28c"),
                RegisteredAt = new DateTime(2020, 1, 3, 10, 33, 22),
                Temperature = 33
            }
        };

        await reporterMediator.Publish(sampleNotification);
    }

    class TestServices : ReporterServices
    {
        private readonly HubConnection _reporterHub;

        public TestServices(HubConnection reporterHub)
        {
            _reporterHub = reporterHub;
        }
        
        public override void RegisterHubs(IServiceCollection services)
        {
            var testHubConnections = new TestHubConnections(_reporterHub);
            services.AddSingleton<IHaveHubConnections>(testHubConnections);
        }
    }
    
    class TestHubConnections : IHaveHubConnections
    {
        private readonly HubConnection _reporterHub;

        public TestHubConnections(HubConnection reporterHub)
        {
            _reporterHub = reporterHub;
        }
        
        public HubConnection GetReporterConnection()
        {
            return _reporterHub;
        }
    }
}