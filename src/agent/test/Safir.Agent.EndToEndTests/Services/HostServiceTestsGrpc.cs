using Google.Protobuf.WellKnownTypes;

namespace Safir.Agent.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class HostServiceTestsGrpc : AgentTestBase
{
    public HostServiceTestsGrpc(AgentFixture service)
        : base(service) { }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var expected = string.Concat(Container.Id.Take(12));

        var result = await Container.CreateHostClient()
            .GetInfoAsync(new Empty());

        Assert.NotNull(result);
        Assert.Equal(expected, result.MachineName);
    }
}
