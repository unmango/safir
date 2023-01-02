using System.Net.Http.Json;
using Safir.Protos;

namespace Safir.Manager.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class HostServiceTestsHttp : ManagerTestBase
{
    public HostServiceTestsHttp(ManagerFixture fixture)
        : base(fixture) { }

    [Fact(Skip = "Need to enable HTTP1 for regular 'ol requests")]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var message = await ManagerContainer.CreateHttpClient().GetAsync("/v1/host/info");
        if (!message.IsSuccessStatusCode) {
            var error = await message.Content.ReadAsStringAsync();
            Assert.Fail(error);
        }

        var result = await message.Content.ReadFromJsonAsync<HostInfo>();

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
