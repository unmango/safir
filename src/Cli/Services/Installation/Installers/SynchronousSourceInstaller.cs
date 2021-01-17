using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal abstract class SynchronousSourceInstaller<T> : ISourceInstaller<T>, ISynchronousSourceInstaller<T>
        where T : IServiceSource
    {
        public abstract bool AppliesTo(ISourceContext context);

        public abstract ISourceInstalled GetInstalled(SourceContext<T> context);

        public ValueTask<ISourceInstalled> GetInstalledAsync(
            SourceContext<T> context,
            CancellationToken cancellationToken = default)
        {
            var installed = GetInstalled(context);
            return ValueTask.FromResult(installed);
        }

        public abstract IServiceUpdate GetUpdate(SourceContext<T> context);

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            SourceContext<T> context,
            CancellationToken cancellationToken = default)
        {
            var update = GetUpdate(context);
            return ValueTask.FromResult(update);
        }

        public abstract void Install(SourceContext<T> context);

        public ValueTask InstallAsync(SourceContext<T> context, CancellationToken cancellationToken = default)
        {
            Install(context);
            return ValueTask.CompletedTask;
        }

        public abstract void Update(SourceContext<T> context);

        public ValueTask UpdateAsync(SourceContext<T> context, CancellationToken cancellationToken = default)
        {
            Update(context);
            return ValueTask.CompletedTask;
        }
    }
}
