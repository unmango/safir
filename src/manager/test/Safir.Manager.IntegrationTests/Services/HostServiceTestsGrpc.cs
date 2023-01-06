using Microsoft.AspNetCore.Mvc.Testing;
using Safir.AspNetCore.Testing;
using Safir.Common.V1Alpha1;

namespace Safir.Manager.IntegrationTests.Services;

[Trait("Category", "Integration")]
public class HostServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HostService.HostServiceClient _client;

    public HostServiceTestsGrpc(WebApplicationFactory<Program> factory)
    {
        var channel = factory.CreateChannel();
        _client = new(channel);
    }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var result = await _client.InfoAsync(new());

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
