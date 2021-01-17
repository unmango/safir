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
            foreach (var source in GetApplicableSources(context))
            {
                var result = await Installer.GetInstalledAsync(source, context, cancellationToken);
                if (result.Installed && result.Source != null)
                {
                    context = context.MarkInstalled(source);
                }
            }

            await next(context);
        }
    }
}
