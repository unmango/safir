using System.IO;
using System.IO.Abstractions;
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
        _configuration = new(
            _optionsMonitor.Object,
            _directory.Object,
            _file.Object,
            Mock.Of<ILogger<JsonConfiguration<FakeOptions>>>());
    }

    [Fact]
    public async Task CreatesConfigurationDirectoryWhenNotExists()
    {
        _optionsMonitor.SetupGet(x => x.CurrentValue).Returns(_defaultOptions);
        _directory
            .Setup(x => x.Exists(_defaultOptions.Config.Directory))
            .Returns(false)
            .Verifiable();
        _file.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream());

        await _configuration.UpdateAsync(_ => { });

        _directory.Verify(x => x.CreateDirectory(_defaultOptions.Config.Directory));
        _directory.Verify();
    }

    public class FakeOptions
    {
        public string Property { get; set; }
    }
}
