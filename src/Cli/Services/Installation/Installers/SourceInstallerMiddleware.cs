using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal sealed class SourceInstallerMiddleware<T> : ISourceInstaller<T>, IServiceInstaller, IInstallationMiddleware
        where T : IServiceSource
    {
        private readonly ISourceInstaller<T> _installer;

        public SourceInstallerMiddleware(ISourceInstaller<T> installer)
        {
            _installer = installer;
        }

        public bool AppliesTo(T context) => _installer.AppliesTo(context);

        public bool AppliesTo(InstallationContext context) => context.Sources.OfType<T>().Any(AppliesTo);

        public ValueTask<IServiceInstalled> GetInstalledAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default)
            => _installer.GetInstalledAsync(context, cancellationToken);

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default)
            => _installer.GetUpdateAsync(context, cancellationToken);

        ValueTask ISourceInstaller<T>.InstallAsync(InstallationContext context, CancellationToken cancellationToken)
            => _installer.InstallAsync(context, cancellationToken);

        ValueTask IServiceInstaller.InstallAsync(InstallationContext context, CancellationToken cancellationToken)
            => _installer.InstallAsync(context, cancellationToken);

        public ValueTask UpdateAsync(InstallationContext context, CancellationToken cancellationToken = default)
            => _installer.UpdateAsync(context, cancellationToken);

        public ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
