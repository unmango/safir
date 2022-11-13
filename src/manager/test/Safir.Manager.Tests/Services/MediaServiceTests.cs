using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Agent.Protos;
using Safir.Manager.Protos;
using Safir.Manager.Services;

namespace Safir.Manager.Tests.Services;

[Trait("Category", "Unit")]
public sealed class MediaServiceTests
{
    private readonly Mock<IAgents> _agents = new();
    private readonly Mock<FileSystem.FileSystemClient> _client1 = new();
    private readonly Mock<FileSystem.FileSystemClient> _client2 = new();
    private readonly Mock<IServerStreamWriter<MediaItem>> _mediaItemWriter = new();
    private readonly Mock<ServerCallContext> _serverCallContext = new();
    private readonly MediaService _service;

    public MediaServiceTests()
    {
        _service = new MediaService(_agents.Object);
    }

    [Fact]
    public async Task List_ListsFiles()
    {
        _client1.Setup(
                x => x.ListFiles(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FileSystemEntry[] { new() { Path = "yeet" } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FileSystem.FileSystemClient> {
            ["Test1"] = _client1.Object,
        });

        await _service.List(new(), _mediaItemWriter.Object, _serverCallContext.Object);

        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = "Test1", Path = "yeet" }, It.IsAny<CancellationToken>()));
    }
}
