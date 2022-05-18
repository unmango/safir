using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Configuration;

public class JsonConfigurationTests
{
    private readonly Mock<IOptionsMonitor<SafirOptions>> _optionsMonitor = new();
    private readonly Mock<IDirectory> _directory = new();
    private readonly Mock<IFile> _file = new();
    private readonly JsonConfiguration<FakeOptions> _configuration;

    private readonly SafirOptions _defaultOptions = new() {
        Config = new() {
            Directory = "test",
        },
    };

    public JsonConfigurationTests()
    {
        _optionsMonitor.SetupGet(x => x.CurrentValue).Returns(_defaultOptions);
        _file.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream());
        _file.Setup(x => x.OpenWrite(It.IsAny<string>())).Returns(new MemoryStream());

        _configuration = new(
            _optionsMonitor.Object,
            _directory.Object,
            _file.Object,
            Mock.Of<ILogger<JsonConfiguration<FakeOptions>>>());
    }

    [Fact]
    public async Task CreatesConfigurationDirectoryWhenDoesntExist()
    {
        _directory.Setup(x => x.Exists(_defaultOptions.Config.Directory))
            .Returns(false)
            .Verifiable();

        await _configuration.UpdateAsync(_ => { });

        _directory.Verify(x => x.CreateDirectory(_defaultOptions.Config.Directory));
        _directory.Verify();
    }

    [Fact]
    public async Task IgnoresConfigurationDirectoryWhenExists()
    {
        _directory.Setup(x => x.Exists(_defaultOptions.Config.Directory))
            .Returns(true)
            .Verifiable();

        await _configuration.UpdateAsync(_ => { });

        _directory.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Never);
        _directory.Verify();
    }

    [Fact]
    public async Task ReadsExistingConfigurationWhenExists()
    {
        var json = Encoding.UTF8.GetBytes("{}");
        await using var stream = new MemoryStream(json);

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(_ => { });

        _file.Verify();
    }

    [Fact]
    public async Task DeserializesExistingConfigurationWhenExists()
    {
        const string expectedValue = "test";
        var json = Encoding.UTF8.GetBytes($"{{\"Property\":\"{expectedValue}\"}}");
        await using var stream = new MemoryStream(json);
        var result = string.Empty;

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(x => result = x.Property);

        Assert.Equal(expectedValue, result);
        _file.Verify();
    }

    [Fact]
    public async Task DeserializesCaseInsensitive()
    {
        const string expectedValue = "test";
        var json = Encoding.UTF8.GetBytes($"{{\"property\":\"{expectedValue}\"}}");
        await using var stream = new MemoryStream(json);
        var result = string.Empty;

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(x => result = x.Property);

        Assert.Equal(expectedValue, result);
        _file.Verify();
    }

    [Fact]
    public async Task DeserializesWithComments()
    {
        const string expectedValue = "test";
        var json = Encoding.UTF8.GetBytes($"// A comment\n{{\"property\":\"{expectedValue}\"}}");
        await using var stream = new MemoryStream(json);
        var result = string.Empty;

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(x => result = x.Property);

        Assert.Equal(expectedValue, result);
        _file.Verify();
    }

    [Fact]
    public async Task CreatesNewConfigurationWhenFileDoesntExist()
    {
        FakeOptions? options = null;

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(false)
            .Verifiable();

        await _configuration.UpdateAsync(x => options = x);

        Assert.NotNull(options);
        _file.Verify();
    }

    [Fact]
    public async Task WritesNewConfigurationWhenFileDoesntExist()
    {
        await using var stream = new MemoryStream();

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(false)
            .Verifiable();
        _file.Setup(x => x.OpenWrite(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(_ => { });
        var result = Encoding.UTF8.GetString(stream.ToArray());

        _file.Verify();
        Assert.Equal("{\n  \"property\": null\n}", result);
    }

    [Fact]
    public async Task WritesConfigurationWhenFileExists()
    {
        const string expectedJson = "{\n  \"property\": null\n}";
        var json = Encoding.UTF8.GetBytes(expectedJson);
        await using var readStream = new MemoryStream(json);
        await using var writeStream = new MemoryStream();

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(readStream)
            .Verifiable();
        _file.Setup(x => x.OpenWrite(_defaultOptions.Config.File))
            .Returns(writeStream)
            .Verifiable();

        await _configuration.UpdateAsync(_ => { });
        var result = Encoding.UTF8.GetString(writeStream.ToArray());

        _file.Verify();
        Assert.Equal(expectedJson, result);
    }

    [Fact]
    public async Task WritesUpdatedConfigurationWhenFileDoesntExist()
    {
        await using var stream = new MemoryStream();
        const string expectedPropertyValue = "test";

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(false)
            .Verifiable();
        _file.Setup(x => x.OpenWrite(_defaultOptions.Config.File))
            .Returns(stream)
            .Verifiable();

        await _configuration.UpdateAsync(x => x.Property = expectedPropertyValue);
        var result = Encoding.UTF8.GetString(stream.ToArray());

        _file.Verify();
        Assert.Equal($"{{\n  \"property\": \"{expectedPropertyValue}\"\n}}", result);
    }

    [Fact]
    public async Task WritesUpdatedConfigurationWhenFileExists()
    {
        const string expectedPropertyValue = "test";
        var json = Encoding.UTF8.GetBytes("{\"Property\":null}");
        await using var readStream = new MemoryStream(json);
        await using var writeStream = new MemoryStream();

        _file.Setup(x => x.Exists(_defaultOptions.Config.File))
            .Returns(true)
            .Verifiable();
        _file.Setup(x => x.OpenRead(_defaultOptions.Config.File))
            .Returns(readStream)
            .Verifiable();
        _file.Setup(x => x.OpenWrite(_defaultOptions.Config.File))
            .Returns(writeStream)
            .Verifiable();

        await _configuration.UpdateAsync(x => x.Property = expectedPropertyValue);
        var result = Encoding.UTF8.GetString(writeStream.ToArray());

        _file.Verify();
        Assert.Equal($"{{\n  \"property\": \"{expectedPropertyValue}\"\n}}", result);
    }

    public class FakeOptions
    {
        public string? Property { get; set; }
    }
}
