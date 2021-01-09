using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installation
{
    internal class DefaultInstallationPipeline : IInstallationPipeline
    {
        private readonly IEnumerable<IInstallationMiddleware> _installers;
        private readonly ILogger<DefaultInstallationPipeline> _logger;

        public DefaultInstallationPipeline(
            IEnumerable<IInstallationMiddleware> installers,
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
