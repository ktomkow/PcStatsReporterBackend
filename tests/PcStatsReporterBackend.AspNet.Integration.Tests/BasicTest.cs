namespace PcStatsReporterBackend.AspNet.Integration.Tests;

public class BasicTest : IntegrationTest
{
    public BasicTest(TestsFixture<Startup> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Test()
    {
        string uri = "/WeatherForecast";
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        var stringResponse = await response.Content.ReadAsStringAsync();

        await Task.Delay(TimeSpan.FromSeconds(5));

        response.EnsureSuccessStatusCode();
        stringResponse.Should().NotBeNull();
    }
}