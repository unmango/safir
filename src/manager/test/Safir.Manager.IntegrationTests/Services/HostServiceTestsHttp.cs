using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.Protos;

namespace Safir.Manager.IntegrationTests.Services;

[Trait("Category", "Integration")]
public class HostServiceTestsHttp : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HostServiceTestsHttp(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var result = await _client.GetFromJsonAsync<HostInfo>("/v1/host/info");

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
