using Google.Protobuf.WellKnownTypes;

namespace Safir.Manager.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class HostServiceTestsGrpc : ManagerTestBase
{
    public HostServiceTestsGrpc(ManagerFixture service)
        : base(service) { }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var expected = string.Concat(ManagerContainer.Id.Take(12));

        var result = await ManagerContainer.CreateHostClient()
            .GetInfoAsync(new Empty());

        Assert.NotNull(result);
        Assert.Equal(expected, result.MachineName);
    }
}
