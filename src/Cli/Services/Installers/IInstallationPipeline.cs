using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installers
{
    internal interface IInstallationPipeline
    {
        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
