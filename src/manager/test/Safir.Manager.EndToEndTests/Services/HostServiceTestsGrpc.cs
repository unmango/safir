using Google.Protobuf.WellKnownTypes;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Collection(ManagerServiceCollection.Name)]
public class HostServiceTestsGrpc : ManagerServiceTestBase
{
    public HostServiceTestsGrpc(ManagerServiceFixture service, ITestOutputHelper output)
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
