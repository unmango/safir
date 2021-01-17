using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal abstract class SynchronousSourceInstaller<T> : ISourceInstaller<T>, ISynchronousSourceInstaller<T>
        where T : IServiceSource
    {
        public abstract bool AppliesTo(T context);

        public abstract ISourceInstalled GetInstalled(T source, InstallationContext context);

        public ValueTask<ISourceInstalled> GetInstalledAsync(
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default)
        {
            var installed = GetInstalled(source, context);
            return ValueTask.FromResult(installed);
        }

        public abstract IServiceUpdate GetUpdate(T source, InstallationContext context);

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default)
        {
            var update = GetUpdate(source, context);
            return ValueTask.FromResult(update);
        }

        public abstract void Install(T source, InstallationContext context);

        public ValueTask InstallAsync(
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default)
        {
            Install(source, context);
            return ValueTask.CompletedTask;
        }

        public abstract void Update(T source, InstallationContext context);

        public ValueTask UpdateAsync(
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default)
        {
            Update(source, context);
            return ValueTask.CompletedTask;
        }
    }
}
