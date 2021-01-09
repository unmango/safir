using System;
using System.Threading;
using System.Threading.Tasks;
using Cli.Services;
using Cli.Services.Installers;
using Microsoft.Extensions.Logging;

namespace Cli.Internal
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class PipelineInstallationService : IInstallationService
    {
        private readonly IServiceDirectory _serviceDirectory;
        private readonly IInstallationPipeline _installationPipeline;
        private readonly ILogger<PipelineInstallationService> _logger;

        public PipelineInstallationService(
            IServiceDirectory serviceDirectory,
            IInstallationPipeline installationPipeline,
            ILogger<PipelineInstallationService> logger)
        {
            _serviceDirectory = serviceDirectory ?? throw new ArgumentNullException(nameof(serviceDirectory));
            _installationPipeline = installationPipeline ?? throw new ArgumentNullException(nameof(installationPipeline));
            _logger = logger;
        }

        public async Task InstallAsync(
            ServiceEntry service,
            string? directory = null,
            CancellationToken cancellationToken = default)
        {
            var workingDirectory = _serviceDirectory.GetInstallationDirectory(directory);

            var context = new InstallationContext(
                workingDirectory,
                service,
                // TODO: Select sources
                service.Sources);

            await _installationPipeline.InstallAsync(context, cancellationToken);
        }
    }
}
