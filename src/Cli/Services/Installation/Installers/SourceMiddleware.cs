using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal abstract class SourceMiddleware<T> : IInstallationMiddleware
        where T : IServiceSource
    {
        protected SourceMiddleware(ISourceInstaller<T> installer)
        {
            Installer = installer;
        }
        
        protected ISourceInstaller<T> Installer { get; }

        public virtual bool AppliesTo(InstallationContext context) => context.Sources.OfType<T>().Any(AppliesTo);

        public abstract ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default);

        protected virtual bool AppliesTo(T source) => Installer.AppliesTo(source);
    }
}
