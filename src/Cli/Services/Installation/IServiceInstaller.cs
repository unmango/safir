using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal interface IServiceInstaller<in T> : IPipeline<InstallationContext>, IAppliesTo<InstallationContext>
        where T : IService
    {
        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);
        
        ValueTask<bool> IsInstalledAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
