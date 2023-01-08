using Grpc.Core;
using Safir.Agent.V1Alpha1;
using Safir.AspNetCore.Testing;
using Safir.Manager.Services;

namespace Safir.Manager.Tests.Services;

[Trait("Category", "Unit")]
public class AgentsExtensionsTests
{
    private readonly Mock<IAgents> _agents = new();
    private readonly Mock<FilesService.FilesServiceClient> _client1 = new();
    private readonly Mock<FilesService.FilesServiceClient> _client2 = new();

    [Fact]
    public async Task ListFilesAsync_SingleAgentListsFiles()
    {
        const string host = "Test1", file = "yeet.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host] = _client1.Object,
        });

        var result = await _agents.Object.ListFilesAsync(CancellationToken.None).ToListAsync();

        Assert.Contains(result, tuple => tuple is { Host: host, Entry.Path: file });
    }

    [Fact]
    public async Task ListFilesAsync_SingleAgentListsAllFiles()
    {
        const string host = "Test1", file1 = "yeet.mp3", file2 = "yolo.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file1 }, new() { Path = file2 } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host] = _client1.Object,
        });

        var result = await _agents.Object.ListFilesAsync(CancellationToken.None).ToListAsync();

        Assert.Contains(result, tuple => tuple is { Host: host, Entry.Path: file1 });
        Assert.Contains(result, tuple => tuple is { Host: host, Entry.Path: file2 });
    }

    [Fact]
    public async Task ListFilesAsync_MultipleAgentsListsFiles()
    {
        const string host1 = "Test1", host2 = "Test2", file = "yeet.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file } });
        _client2.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host1] = _client1.Object,
            [host2] = _client2.Object,
        });

        var result = await _agents.Object.ListFilesAsync(CancellationToken.None).ToListAsync();

        Assert.Contains(result, tuple => tuple is { Host: host1, Entry.Path: file });
        Assert.Contains(result, tuple => tuple is { Host: host2, Entry.Path: file });
    }

    [Fact]
    public async Task ListFilesAsync_MultipleAgentListsAllFiles()
    {
        const string host1 = "Test1", host2 = "Test2", file1 = "yeet.mp3", file2 = "yolo.mp3";
        _client1.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file1 }, new() { Path = file2 } });
        _client2.Setup(x => x.List(
                It.IsAny<FilesServiceListRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilesServiceListResponse[] { new() { Path = file1 }, new() { Path = file2 } });

        _agents.SetupGet(x => x.FileSystem).Returns(new Dictionary<string, FilesService.FilesServiceClient> {
            [host1] = _client1.Object,
            [host2] = _client2.Object,
        });

        var result = await _agents.Object.ListFilesAsync(CancellationToken.None).ToListAsync();

        Assert.Contains(result, tuple => tuple is { Host: host1, Entry.Path: file1 });
        Assert.Contains(result, tuple => tuple is { Host: host1, Entry.Path: file2 });
        Assert.Contains(result, tuple => tuple is { Host: host2, Entry.Path: file1 });
        Assert.Contains(result, tuple => tuple is { Host: host2, Entry.Path: file2 });
    }
}
