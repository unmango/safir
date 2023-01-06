using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.Common.V1Alpha1;

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
        var result = await _client.GetFromJsonAsync<InfoResponse>("/v1/host/info");

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
