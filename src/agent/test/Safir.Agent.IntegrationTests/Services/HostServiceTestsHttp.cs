using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.IntegrationTests.Services;

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
        var message = await _client.GetAsync("/v1/host/info");
        if (!message.IsSuccessStatusCode) {
            var error = await message.Content.ReadAsStringAsync();
            Assert.Fail(error);
        }

        var result = await message.Content.ReadFromJsonAsync<InfoResponse>();

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
