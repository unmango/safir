using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installation
{
    internal class PipelineServiceInstaller : IServiceInstaller
    {
        private readonly IServiceDirectory _serviceDirectory;
        private readonly IEnumerable<IInstallationMiddleware> _middleware;
        private readonly ILogger<PipelineServiceInstaller> _logger;

        public PipelineServiceInstaller(
            IServiceDirectory serviceDirectory,
            IEnumerable<IInstallationMiddleware> middleware,
            ILogger<PipelineServiceInstaller> logger)
        {
            _serviceDirectory = serviceDirectory;
            _middleware = middleware;
            _logger = logger;
        }

        public ValueTask<ServiceInstalled> GetInstalledAsync(
            IService service,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task InstallAsync(
            IService service,
            string? directory = null,
            CancellationToken cancellationToken = default)
        {
            _logger.InvokedWorkingDirectory(directory);
            var workingDirectory = _serviceDirectory.GetInstallationDirectory(directory);
            _logger.ResolvedWorkingDirectory(workingDirectory);
            
            var context = new InstallationContext(workingDirectory, service, service.Sources);
            
            _logger.InitialContextCreated(context);

            _logger.InitialInstallers(_middleware);
            var applicable = _middleware.AllApplicable(context).ToList();
            _logger.ApplicableInstallers(applicable);

            if (applicable.Count <= 0) return Task.CompletedTask;
            
            var pipeline = applicable.BuildPipeline();
            return pipeline.InvokeAsync(context, cancellationToken).AsTask();
        }
    }
}
