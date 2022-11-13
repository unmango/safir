using Google.Protobuf.WellKnownTypes;
using Xunit.Abstractions;

namespace Safir.Agent.EndToEndTests.Services;

[Collection(AgentServiceCollection.Name)]
[Trait("Category", "EndToEnd")]
public class HostServiceTestsGrpc : AgentServiceTestBase
{
    public HostServiceTestsGrpc(AgentServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var expected = string.Concat(Container.Id.Take(12));
        var result = await GetHostClient().GetInfoAsync(new Empty());

        Assert.NotNull(result);
        Assert.Equal(expected, result.MachineName);
    }
}
