using System.IO.Abstractions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Agent.Protos;
using Safir.Agent.Services;

namespace Safir.Agent.Tests.Services;

public class FileSystemServiceTests
{
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

        _service = new(_options.Object, fileSystem.Object, Mock.Of<ILogger<FileSystemService>>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public async Task SendsEmptyWhenNoRoot(string? root)
    {
        _options.SetupGet(x => x.Value).Returns(new AgentConfiguration {
            DataDirectory = root,
        });

        await _service.ListFiles(new(), _serverStreamWriter.Object, null!);

        Assert.NotNull(result);
        Assert.Empty(result.Files);
        _mock.GetMock<IDirectory>().VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendsEmptyWhenDirectoryDoesNotExist()
    {
        const string dir = "dir";
        _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(false);
        var request = new ListFilesRequest(dir);

        var result = await _handler.Handle(request, default);

        Assert.NotNull(result);
        Assert.Empty(result.Files);
        _mock.GetMock<IDirectory>().VerifyAll();
        _mock.GetMock<IDirectory>().VerifyNoOtherCalls();
    }

    [Fact]
    public async Task EnumeratesWithWildCardFilter()
    {
        const string dir = "dir";
        _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
        var request = new ListFilesRequest(dir);

        await _handler.Handle(request, default);

        _mock.GetMock<IDirectory>()
            .Verify(x => x.EnumerateFileSystemEntries(dir, "*", It.IsAny<EnumerationOptions>()));
    }

    [Fact]
    public async Task PassesEnumerationOptionsFromRequest()
    {
        const string dir = "dir";
        _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
        var options = new EnumerationOptions();
        var request = new ListFilesRequest(dir, options);

        await _handler.Handle(request, default);

        _mock.GetMock<IDirectory>().Verify(x => x.EnumerateFileSystemEntries(
            dir,
            It.IsAny<string>(),
            It.Is<EnumerationOptions>(y => y == options)));
    }

    [Fact]
    public async Task ReturnsPathRelativeToRoot()
    {
        const string dir = "dir", entry = "entry", relative = "relative";
        _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
        _mock.Setup<IDirectory, IEnumerable<string>>(x =>
                x.EnumerateFileSystemEntries(dir, It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
            .Returns(new[] { entry });
        _mock.Setup<IPath, string>(x => x.GetRelativePath(dir, entry)).Returns(relative);
        var request = new ListFilesRequest(dir);

        var result = await _handler.Handle(request, default);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Files);
        var paths = result.Files.Select(x => x.Path).ToList();
        Assert.Contains(relative, paths);
        Assert.DoesNotContain(entry, paths);
    }
}
