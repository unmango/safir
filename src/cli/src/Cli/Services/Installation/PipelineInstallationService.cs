using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installation
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
            IService service,
            string? directory = null,
            CancellationToken cancellationToken = default)
        {
            _logger.InvokedWorkingDirectory(directory);
            var workingDirectory = _serviceDirectory.GetInstallationDirectory(directory);
            _logger.ResolvedWorkingDirectory(workingDirectory);

            var context = new InstallationContext(
                workingDirectory,
                service,
                // TODO: Select sources
                service.Sources);
            
            _logger.InitialContextCreated(context);

            await _installationPipeline.InstallAsync(context, cancellationToken);
        }
    }
}
