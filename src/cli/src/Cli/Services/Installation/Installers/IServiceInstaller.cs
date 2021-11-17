using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal delegate ValueTask InstallAsync(
        InstallationContext context,
        CancellationToken cancellationToken = default);

    internal interface IServiceInstaller
    {
        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
