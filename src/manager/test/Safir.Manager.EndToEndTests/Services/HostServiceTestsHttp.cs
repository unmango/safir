using System.Net.Http.Json;
using Safir.Protos;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Collection(ManagerServiceCollection.Name)]
public class HostServiceTestsHttp : ManagerServiceTestBase
{
    public HostServiceTestsHttp(ManagerServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    [Fact]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var result = await GetHttpClient().GetFromJsonAsync<HostInfo>("/v1/host/info");

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
