using System.Net.Http.Json;
using Safir.Protos;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Collection(ManagerServiceCollection.Name)]
[Trait("Category", "EndToEnd")]
public class HostServiceTestsHttp : ManagerServiceTestBase
{
    public HostServiceTestsHttp(ManagerServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    [Fact(Skip = "Need to enable HTTP1 for regular 'ol requests")]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var message = await GetHttpClient().GetAsync("/v1/host/info");
        if (!message.IsSuccessStatusCode) {
            var error = await message.Content.ReadAsStringAsync();
            Assert.Fail(error);
        }

        var result = await message.Content.ReadFromJsonAsync<HostInfo>();

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
