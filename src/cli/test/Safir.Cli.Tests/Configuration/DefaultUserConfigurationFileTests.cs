using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Configuration;

public class DefaultUserConfigurationFileTests
{
    private const string FilePath = "/my/test/file";

    private readonly Mock<IOptions<SafirOptions>> _options = new();
    private readonly Mock<IPath> _path = new();
    private readonly Mock<IDirectory> _directory = new();
    private readonly Mock<IFile> _file = new();
    private readonly DefaultUserConfigurationFile _configurationFile;

    private readonly SafirOptions _defaultOptions = new() {
        Config = new() {
            File = FilePath,
        },
    };

    public DefaultUserConfigurationFileTests()
    {
        _options.SetupGet(x => x.Value).Returns(_defaultOptions);
        _file.Setup(x => x.OpenWrite(It.IsAny<string>())).Returns(new MemoryStream());

        _configurationFile = new DefaultUserConfigurationFile(
            _options.Object,
            _path.Object,
            _directory.Object,
            _file.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Exists_UsesConfiguredFilePath(bool exists)
    {
        _file.Setup(x => x.Exists(FilePath)).Returns(exists).Verifiable();

        var result = _configurationFile.Exists;

        _file.Verify();
        _file.VerifyNoOtherCalls();
        Assert.Equal(exists, result);
    }

    [Fact]
    public void GetReader_ThrowsWhenFileDoesntExist()
    {
        _file.Setup(x => x.Exists(FilePath)).Returns(false).Verifiable();

        Assert.Throws<InvalidOperationException>(() => _configurationFile.GetReader());
        _file.Verify();
        _file.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetReader_GetsReaderForConfiguredFile()
    {
        var expectedText = Encoding.UTF8.GetBytes("Test Text");
        await using var stream = new MemoryStream(expectedText);
        _file.Setup(x => x.Exists(FilePath)).Returns(true).Verifiable();
        _file.Setup(x => x.OpenRead(FilePath)).Returns(stream).Verifiable();

        var reader = _configurationFile.GetReader();

        _file.Verify();
        _file.VerifyNoOtherCalls();
        var readResult = await reader.ReadAsync();
        var result = readResult.Buffer.ToArray();
        Assert.Equal(expectedText, result);
    }

    [Fact]
    public void GetWriter_ThrowsWhenPathIsADirectory()
    {
        _path.Setup(x => x.EndsInDirectorySeparator(FilePath)).Returns(true).Verifiable();

        Assert.Throws<InvalidOperationException>(() => _configurationFile.GetWriter());
        _path.Verify();
        _path.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetWriter_ThrowsWhenPathIsNotAbsolute()
    {
        _path.Setup(x => x.EndsInDirectorySeparator(It.IsAny<string>())).Returns(false).Verifiable();
        _path.Setup(x => x.IsPathRooted(FilePath)).Returns(false).Verifiable();

        Assert.Throws<InvalidOperationException>(() => _configurationFile.GetWriter());
        _path.Verify();
        _path.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetWriter_CreatesDirectoryWhenItDoesntExist()
    {
        const string directory = "/my/directory/";
        _path.Setup(x => x.EndsInDirectorySeparator(It.IsAny<string>())).Returns(false);
        _path.Setup(x => x.IsPathRooted(It.IsAny<string>())).Returns(true);
        _path.Setup(x => x.GetDirectoryName(FilePath)).Returns(directory).Verifiable();
        _directory.Setup(x => x.Exists(directory)).Returns(false).Verifiable();

        _ = _configurationFile.GetWriter();

        _path.Verify();
        _directory.Verify();
        _directory.Verify(x => x.CreateDirectory(directory));
        _directory.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetWriter_IgnoresDirectoryWhenItExists()
    {
        const string directory = "/my/directory/";
        _path.Setup(x => x.EndsInDirectorySeparator(It.IsAny<string>())).Returns(false);
        _path.Setup(x => x.IsPathRooted(It.IsAny<string>())).Returns(true);
        _path.Setup(x => x.GetDirectoryName(FilePath)).Returns(directory).Verifiable();
        _directory.Setup(x => x.Exists(directory)).Returns(true).Verifiable();

        _ = _configurationFile.GetWriter();

        _path.Verify();
        _directory.Verify();
        _directory.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetWriter_CreatesWriterForConfigurationFile()
    {
        var expectedText = Encoding.UTF8.GetBytes("Test Text");
        await using var stream = new MemoryStream();
        _path.Setup(x => x.EndsInDirectorySeparator(It.IsAny<string>())).Returns(false);
        _path.Setup(x => x.IsPathRooted(It.IsAny<string>())).Returns(true);
        _file.Setup(x => x.OpenWrite(FilePath)).Returns(stream).Verifiable();

        var writer = _configurationFile.GetWriter();

        await writer.WriteAsync(expectedText);
        Assert.Equal(expectedText, stream.ToArray());
    }
}
