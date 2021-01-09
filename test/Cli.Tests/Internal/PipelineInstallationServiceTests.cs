using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal;
using Cli.Services;
using Cli.Services.Installers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Cli.Tests.Internal
{
    public class PipelineInstallationServiceTests
    {
        private const string WorkingDirectory = "workingDirectory";
        private static readonly AutoMocker _mocker = new();
        private readonly ServiceEntry _defaultService = new();
        private readonly PipelineInstallationService _service = _mocker.Get<PipelineInstallationService>();

        public PipelineInstallationServiceTests()
        {
            _mocker.GetMock<IServiceDirectory>()
                .Setup(x => x.GetInstallationDirectory(It.IsAny<IEnumerable<string>?>()))
                .Returns(WorkingDirectory);
        }

        // TODO: Probs break this out into two tests
        [Theory]
        [InlineData((string?)null)]
        [InlineData("directory")]
        public async Task PassesDirectory(string? directory)
        {
            if (directory == null)
                await _service.InstallAsync(_defaultService);
            else
                await _service.InstallAsync(_defaultService, directory);

            _mocker.GetMock<IServiceDirectory>().Verify(x => x.GetInstallationDirectory(It.Is<IEnumerable<string>>(
                directories => directory == null
                    ? !directories.Any()
                    : directories.Contains(directory))));
        }

        [Fact]
        public async Task InvokesPipeline()
        {
            var expectedSources = new[] { new ServiceSource() };
            var service = _defaultService with { Sources = expectedSources };
            var pipeline = _mocker.GetMock<IInstallationPipeline>();

            await _service.InstallAsync(service);

            pipeline.Verify(x => x.InstallAsync(
                It.Is<InstallationContext>(context =>
                    context.WorkingDirectory == WorkingDirectory &&
                    context.Service == service &&
                    context.Sources.Equals(expectedSources)),
                It.IsAny<CancellationToken>()));
        }
    }
}
