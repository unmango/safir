using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installers
{
    internal abstract class PipelineServiceInstaller : IServiceInstaller, IPipelineServiceInstaller
    {
        public abstract bool AppliesTo(InstallationContext context);

        public virtual ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default)
            => AppliesTo(context)
                ? InstallAsync(context, cancellationToken)
                : next(context);

        public abstract ValueTask InstallAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default);
    }
}
