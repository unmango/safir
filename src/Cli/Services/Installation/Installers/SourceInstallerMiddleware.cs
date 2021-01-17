using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal sealed class SourceInstallerMiddleware<T> : SourceMiddleware<T>
        where T : IServiceSource
    {
        public SourceInstallerMiddleware(ISourceInstaller<T> installer) : base(installer)
        {
        }

        public override async ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default)
        {
            // TODO: Install all applicable?
            foreach (var source in GetApplicableSources(context))
            {
                await Installer.InstallAsync(source, context, cancellationToken);
                
                // TODO: Check installed? Return result?
                context.MarkInstalled(source);
            }

            await next(context);
        }
    }
}
