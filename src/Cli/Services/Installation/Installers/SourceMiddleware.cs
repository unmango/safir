using System;
using System.Collections.Generic;
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

        public virtual bool AppliesTo(InstallationContext context) => GetApplicableSources(context).Any();

        public abstract ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default);

        protected virtual bool AppliesTo(T source) => Installer.AppliesTo(source);

        protected IEnumerable<T> GetApplicableSources(InstallationContext context)
            => context.Sources.OfType<T>().Where(AppliesTo);
    }
}
