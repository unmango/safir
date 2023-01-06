using System.Net.Http.Json;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class HostServiceTestsHttp : AgentTestBase
{
    public HostServiceTestsHttp(AgentFixture service)
        : base(service) { }

    [Fact(Skip = "Need to enable HTTP1 for regular 'ol requests")]
    public async Task GetInfo_ReturnsHostInfo()
    {
        var message = await Container.CreateHttpClient()
            .GetAsync("/v1/host/info");

        if (!message.IsSuccessStatusCode) {
            var error = await message.Content.ReadAsStringAsync();
            Assert.Fail(error);
        }

        var result = await message.Content.ReadFromJsonAsync<InfoResponse>();

        Assert.NotNull(result);
        Assert.Equal(Environment.MachineName, result.MachineName);
    }
}
