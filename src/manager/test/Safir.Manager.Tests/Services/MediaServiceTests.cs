using Grpc.Core;
using Safir.Agent.V1Alpha1;
using Safir.AspNetCore.Testing;
using Safir.Manager.V1Alpha1;
using ListRequest = Safir.Agent.V1Alpha1.ListRequest;
using ListResponse = Safir.Agent.V1Alpha1.ListResponse;
using MediaService = Safir.Manager.Services.MediaService;

namespace Safir.Manager.Tests.Services;

[Trait("Category", "Unit")]
public sealed class MediaServiceTests
{
    private readonly Mock<IAgents> _agents = new();
    private readonly Mock<FilesService.FilesServiceClient> _client1 = new();
    private readonly Mock<FilesService.FilesServiceClient> _client2 = new();
    private readonly Mock<IServerStreamWriter<MediaItem>> _mediaItemWriter = new();
    private readonly ServerCallContext _callContext = Mock.Of<ServerCallContext>();
    private readonly MediaService _service;

    public MediaServiceTests()
    {
        _service = new MediaService(_agents.Object);
    }

    [Fact]
    public async Task List_SingleAgentListsFiles()
    {
        const string host = "Test1", file = "yeet.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host] = _client1.Object,
        });

        await _service.List(new(), _mediaItemWriter.Object, _callContext);

        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host, Path = file }, _callContext.CancellationToken));
    }

    [Fact]
    public async Task List_SingleAgentListsAllFiles()
    {
        const string host = "Test1", file1 = "yeet.mp3", file2 = "yolo.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file1 }, new() { Path = file2 } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host] = _client1.Object,
        });

        await _service.List(new(), _mediaItemWriter.Object, _callContext);

        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host, Path = file1 }, _callContext.CancellationToken));
        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host, Path = file2 }, _callContext.CancellationToken));
    }

    [Fact]
    public async Task List_MultipleAgentsListsFiles()
    {
        const string host1 = "Test1", host2 = "Test2", file = "yeet.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file } });
        _client2.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host1] = _client1.Object,
            [host2] = _client2.Object,
        });

        await _service.List(new(), _mediaItemWriter.Object, _callContext);

        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host1, Path = file }, _callContext.CancellationToken));
        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host2, Path = file }, _callContext.CancellationToken));
    }

    [Fact]
    public async Task List_MultipleAgentListsAllFiles()
    {
        const string host1 = "Test1", host2 = "Test2", file1 = "yeet.mp3", file2 = "yolo.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file1 }, new() { Path = file2 } });
        _client2.Setup(x => x.List(
                It.IsAny<ListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                _callContext.CancellationToken))
            .ReturnsAsync(new ListResponse[] { new() { Path = file1 }, new() { Path = file2 } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host1] = _client1.Object,
            [host2] = _client2.Object,
        });

        await _service.List(new(), _mediaItemWriter.Object, _callContext);

        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host1, Path = file1 }, _callContext.CancellationToken));
        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host1, Path = file2 }, _callContext.CancellationToken));
        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host2, Path = file1 }, _callContext.CancellationToken));
        _mediaItemWriter.Verify(x => x.WriteAsync(new() { Host = host2, Path = file2 }, _callContext.CancellationToken));
    }
}
