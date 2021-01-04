using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal interface IServiceInstaller
    {
        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
