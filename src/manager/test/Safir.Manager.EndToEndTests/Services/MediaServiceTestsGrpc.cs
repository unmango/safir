using DotNet.Testcontainers.Configurations;
using Google.Protobuf.WellKnownTypes;
using Safir.Grpc;
using Safir.XUnit.AspNetCore;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class MediaServiceTestsGrpc : ManagerTestBase
{
    public MediaServiceTestsGrpc(ManagerFixture fixture, ITestOutputHelper output)
        : base(fixture)
    {
        TestcontainersSettings.Logger = new TestOutputHelperLogger(output);
    }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        const string fileName = "Test.txt";
        await AgentContainer.CreateMediaFileAsync(fileName);

        var result = await ManagerContainer.CreateMediaClient()
            .ListAsync(new());

        var item = Assert.Single(result.Media);
        Assert.Equal(AgentName, item.Host);
        Assert.Equal(fileName, item.Path);
    }
}
