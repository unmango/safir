using Google.Protobuf.WellKnownTypes;
using Safir.Grpc;

namespace Safir.Agent.EndToEndTests.Services;

[Trait("Category", "EndToEnd")]
public class FileSystemServiceTestsGrpc : AgentTestBase
{
    public FileSystemServiceTestsGrpc(AgentFixture service)
        : base(service) { }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        await Container.ExecAsync(new[] { "touch", "/data/Test.mp3" });

        var result = await Container.CreateFileSystemClient()
            .ListFiles(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test.mp3", item.Path);
    }
}
