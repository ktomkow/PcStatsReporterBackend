using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;

namespace PcStatsReporterBackend.AspNet.Integration.Tests;

[Collection(nameof(IntegrationTest))]
public abstract class IntegrationTest : IClassFixture<TestsFixture<Startup>>
{
    private const string mainHubEndpoint = "main"; 
    
    protected readonly TestsFixture<Startup> _fixture;
    protected readonly HttpClient _httpClient;
    protected readonly HubConnection _mainHubConnection;
    protected readonly CancellationTokenSource _delayTokenSource;
    private CancellationToken _delayToken => _delayTokenSource.Token;

    public IntegrationTest(TestsFixture<Startup> fixture)
    {
        _fixture = fixture;
        WebApplicationFactoryClientOptions webAppFactoryClientOptions = new WebApplicationFactoryClientOptions
        {
            // Disallow redirect so that we can check the following: Status code is redirect and redirect url is login url
            // As per https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2#test-a-secure-endpoint
            AllowAutoRedirect = false
        };

        _httpClient = fixture.CreateClient(webAppFactoryClientOptions);
        
        Uri baseAddress = _fixture.Server.BaseAddress;
        string address = baseAddress + mainHubEndpoint;
        
        // https://lurumad.github.io/integration-tests-in-aspnet-core-signalr
        _mainHubConnection = new HubConnectionBuilder()
            .WithUrl(address, o => o.HttpMessageHandlerFactory = _ => _fixture.Server.CreateHandler())
            .Build();
        
        _delayTokenSource = new CancellationTokenSource();
    }

    private async Task Delay(TimeSpan timeSpan)
    {
        try
        {
            await Task.Delay(timeSpan, _delayToken);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    protected async Task Delay(uint seconds)
    {
        await Delay(TimeSpan.FromSeconds(seconds));
    }
}