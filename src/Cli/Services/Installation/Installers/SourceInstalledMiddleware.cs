using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal class SourceInstalledMiddleware<T> : SourceMiddleware<T>
        where T : IServiceSource
    {
        public SourceInstalledMiddleware(ISourceInstaller<T> installer) : base(installer)
        {
        }

        public override async ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default)
        {
            var result = await Installer.GetInstalledAsync(context, cancellationToken);
            if (result.Installed && result.Source != null)
            {
                context = context.WithProperty(SourceInstalled.Key(result.Source), true);
            }

            await next(context);
        }
    }
}
