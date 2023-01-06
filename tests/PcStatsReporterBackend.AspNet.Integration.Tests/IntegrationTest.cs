using Microsoft.AspNetCore.Mvc.Testing;

namespace PcStatsReporterBackend.AspNet.Integration.Tests;

[Collection(nameof(IntegrationTest))]
public abstract class IntegrationTest : IClassFixture<TestsFixture<Startup>>
{
    protected readonly TestsFixture<Startup> _fixture;
    protected readonly HttpClient _httpClient;

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
    }
}