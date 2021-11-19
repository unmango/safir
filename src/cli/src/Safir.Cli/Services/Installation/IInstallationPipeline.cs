using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Installation
{
    internal interface IInstallationPipeline
    {
        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
