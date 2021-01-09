using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installers
{
    internal class DefaultInstallationPipeline : IInstallationPipeline
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly IEnumerable<IPipelineServiceInstaller> _installers;
        private readonly ILogger<DefaultInstallationPipeline> _logger;

        public DefaultInstallationPipeline(
            IEnumerable<IPipelineServiceInstaller> installers,
            ILogger<DefaultInstallationPipeline> logger)
        {
            _installers = installers;
            _logger = logger;
        }

        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            _logger.InitialInstallers(_installers);
            var applicable = _installers.Where(x => x.AppliesTo(context)).ToList();
            _logger.ApplicableInstallers(applicable);

            if (applicable.Count <= 0) return ValueTask.CompletedTask;

            return applicable.BuildPipeline().Invoke(
                context,
                _ => ValueTask.CompletedTask,
                cancellationToken);
        }
    }
}
