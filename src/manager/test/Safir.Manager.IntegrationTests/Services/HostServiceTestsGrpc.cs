using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.AspNetCore.IntegrationTesting;
using Safir.Protos;

namespace Safir.Manager.IntegrationTests.Services;

public class HostServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Host.HostClient _client;

    public HostServiceTestsGrpc(WebApplicationFactory<Program> factory)
    {
        var channel = factory.CreateChannel();
        _client = new Host.HostClient(channel);
    }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var result = await _client.GetInfoAsync(new Empty());

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
