using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using PcStatsReporterBackend.AspNet.SignalR;
using PcStatsReporterBackend.Contracts;
using PcStatsReporterBackend.Contracts.ToSubscriber;
using PcStatsReporterBackend.Core;
using PcStatsReporterBackend.Reporter;
using PcStatsReporterBackend.Reporter.Features.SamplePublisher;
using PcStatsReporterBackend.Reporter.Features.SignalR;

namespace PcStatsReporterBackend.AspNet.Integration.Tests.SignalRTests;

public class SignalRPublisherTests: IntegrationTest
{
    private readonly IMediator _reporterMediator;
    private readonly HubConnection _reporterHubConnection;

    public SignalRPublisherTests(TestsFixture<Startup> fixture) : base(fixture)
    {
        _reporterHubConnection = GetReporterHubConnection();
        var testServices = new TestServices(_reporterHubConnection);
        var reporterBuilder = BasicReporter.CreateHostBuilder(testServices);
        var reporterHost = reporterBuilder.Build();
        _reporterMediator = reporterHost.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Publish_IfSamplePublished_ConfirmationShouldCome()
    {
        // arrange
        SampleConfirmation sampleConfirmation = default(SampleConfirmation)!;
        
        _reporterHubConnection.On<SampleConfirmation>(ReporterHub.OnSampleReceived, (response) =>
        {
            sampleConfirmation = response;
            _delayTokenSource.Cancel();
        });

        var sampleNotification = new SampleNotification()
        {
            Sample = new CpuSample()
            {
                Id = Guid.Parse("00203459-cfd9-4d28-a83c-6b5dc5c6e28c"),
                RegisteredAt = new DateTime(2020, 1, 3, 10, 33, 22),
                Temperature = 33
            }
        };
        
        // act
        await _reporterMediator.Publish(sampleNotification);
        
        await Delay(30);
        
        // assert
        sampleConfirmation.Should().NotBeNull();
        sampleConfirmation.IsSuccess.Should().BeTrue();
        sampleConfirmation.MessageId.Should().NotBe(default(Guid));
    }
    
    [Fact]
    public async Task Publish_WhenSubscribed_ThenGetSample()
    {
        // arrange
        var subscriberConnectionHub = GetReporterHubConnection();
        CpuSampleDto resultSampleDto = default(CpuSampleDto)!;
        
        subscriberConnectionHub.On<CpuSampleDto>(ReporterHub.OnCpuSample, (response) =>
        {
            resultSampleDto = response;
            _delayTokenSource.Cancel();
        });

        var cpuSample = new CpuSample()
        {
            Id = Guid.Parse("00203459-cfd9-4d28-a83c-6b5dc5c6e28c"),
            RegisteredAt = new DateTime(2020, 1, 3, 10, 33, 22),
            Temperature = 35
        };

        var sampleNotification = new SampleNotification()
        {
            Sample = cpuSample
        };

        await subscriberConnectionHub.StartAsync();
        await subscriberConnectionHub.SendAsync(ReporterHub.SubscribeFunction);
        
        // act
        await _reporterMediator.Publish(sampleNotification);
        
        await Delay(30);
        
        // assert
        resultSampleDto.Should().NotBeNull();
        resultSampleDto.Temperature.Should().Be(cpuSample.Temperature);
        resultSampleDto.Id.Should().Be(cpuSample.Id);
        resultSampleDto.RegisteredAt.Should().BeCloseTo(cpuSample.RegisteredAt, TimeSpan.FromMilliseconds(100));

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