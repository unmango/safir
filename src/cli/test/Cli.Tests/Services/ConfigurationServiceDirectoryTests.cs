using Cli.Services;
using Microsoft.Extensions.Options;
using Moq.AutoMock;
using Xunit;

namespace Cli.Tests.Services
{
    public class ConfigurationServiceDirectoryTests
    {
        // TODO: Your machine too!
        private const string ConfigDir = "/home/erik/.safir";
        private static readonly AutoMocker _mocker = new();

        private readonly ConfigurationServiceDirectory _serviceDirectory =
            _mocker.CreateInstance<ConfigurationServiceDirectory>();

        private readonly CliOptions _options = new() {
            Config = new ConfigOptions {
                Directory = ConfigDir
            }
        };

        public ConfigurationServiceDirectoryTests()
        {
            _mocker.GetMock<IOptions<CliOptions>>()
                .SetupGet(x => x.Value)
                .Returns(_options);
        }

        [Fact]
        public void SelectsValidRootedPathParam()
        {
            // TODO: On your machine too!
            const string path = "/home";

            var result = _serviceDirectory.GetInstallationDirectory(new[] { path });

            Assert.Equal(path, result);
        }

        [Fact]
        public void UsesDefaultInstallDirectory()
        {
            var expected = $"{ConfigDir}/services";

            var result = _serviceDirectory.GetInstallationDirectory();
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public void JoinsPartWithDefaultInstallationDirectory()
        {
            const string part = "part";
            var expected = $"{ConfigDir}/services/{part}";

            var result = _serviceDirectory.GetInstallationDirectory(new[] { part });
            
            Assert.Equal(expected, result);
        }
    }
}
