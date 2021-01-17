using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal abstract class SynchronousSourceInstaller<T> : ISourceInstaller<T>, ISynchronousSourceInstaller<T>
        where T : IServiceSource
    {
        public abstract bool AppliesTo(T context);

        public abstract ISourceInstalled GetInstalled(InstallationContext context);

        public ValueTask<ISourceInstalled> GetInstalledAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            var installed = GetInstalled(context);
            return ValueTask.FromResult(installed);
        }

        public abstract IServiceUpdate GetUpdate(InstallationContext context);

        public ValueTask<IServiceUpdate> GetUpdateAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            var update = GetUpdate(context);
            return ValueTask.FromResult(update);
        }

        public abstract void Install(InstallationContext context);

        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            Install(context);
            return ValueTask.CompletedTask;
        }

        public abstract void Update(InstallationContext context);

        public ValueTask UpdateAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            Update(context);
            return ValueTask.CompletedTask;
        }
    }
}
