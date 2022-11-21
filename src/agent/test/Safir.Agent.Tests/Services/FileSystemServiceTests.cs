using System.IO.Abstractions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Agent.Protos;
using Safir.Agent.Services;

namespace Safir.Agent.Tests.Services;

[Trait("Category", "Unit")]
public class FileSystemServiceTests
{
    private const string Directory = "dir";
    private readonly Mock<IOptions<AgentConfiguration>> _options = new();
    private readonly Mock<IDirectory> _directory = new();
    private readonly Mock<IPath> _path = new();
    private readonly Mock<IServerStreamWriter<FileSystemEntry>> _serverStreamWriter = new();
    private readonly FileSystemService _service;

    public FileSystemServiceTests()
    {
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.SetupGet(x => x.Directory).Returns(_directory.Object);
        fileSystem.SetupGet(x => x.Path).Returns(_path.Object);

        _options.SetupGet(x => x.Value).Returns(new AgentConfiguration {
            DataDirectory = Directory,
        });

        _service = new(_options.Object, fileSystem.Object, Mock.Of<ILogger<FileSystemService>>());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public async Task SendsEmptyWhenNoRoot(string? root)
    {
        _options.SetupGet(x => x.Value).Returns(new AgentConfiguration {
            DataDirectory = root,
        });

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        _directory.VerifyNoOtherCalls();
        _serverStreamWriter.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendsEmptyWhenDirectoryDoesNotExist()
    {
        _directory.Setup(x => x.Exists(Directory)).Returns(false);

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        _serverStreamWriter.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task EnumeratesWithWildCardFilter()
    {
        _directory.Setup(x => x.Exists(Directory)).Returns(true);

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        _directory.Verify(x => x.EnumerateFileSystemEntries(Directory, "*"));
    }

    [Fact]
    public async Task ReturnsPathRelativeToRoot()
    {
        const string entry = "entry";
        _directory.Setup(x => x.Exists(Directory)).Returns(true);
        _directory.Setup(x => x.EnumerateFileSystemEntries(Directory, It.IsAny<string>()))
            .Returns(new[] { entry });
        _path.Setup(x => x.GetRelativePath(Directory, entry)).Returns("bogus");

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        _path.Verify(x => x.GetRelativePath(Directory, entry));
    }

    [Fact]
    public async Task WritesAllEntries()
    {
        const string entry = "entry", relative = "relative";
        _directory.Setup(x => x.Exists(Directory)).Returns(true);
        _directory.Setup(x => x.EnumerateFileSystemEntries(Directory, It.IsAny<string>()))
            .Returns(new[] { entry });
        _path.Setup(x => x.GetRelativePath(Directory, entry)).Returns(relative);

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        _serverStreamWriter.Verify(x => x.WriteAsync(It.Is<FileSystemEntry>(f => f.Path == relative)));
    }
}
