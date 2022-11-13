using Google.Protobuf.WellKnownTypes;
using Safir.Grpc;
using Xunit.Abstractions;

namespace Safir.Agent.EndToEndTests.Services;

[Collection(AgentServiceCollection.Name)]
[Trait("Category", "EndToEnd")]
public class FileSystemServiceTestsGrpc : AgentServiceTestBase
{
    public FileSystemServiceTestsGrpc(AgentServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        await Container.ExecAsync(new[] { "touch", "/data/Test.mp3" });

        var result = await GetFileSystemClient().ListFiles(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test.mp3", item.Path);
    }
}
