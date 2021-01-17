using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    // TODO: Consider changing to AppliesTo<InstallationContext>
    internal interface ISourceInstaller<in T> : IAppliesTo<T>
        where T : IServiceSource
    {
        ValueTask<ISourceInstalled> GetInstalledAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default);

        ValueTask<IServiceUpdate> GetUpdateAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default);

        ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default);

        ValueTask UpdateAsync(InstallationContext context, CancellationToken cancellationToken = default);
    }
}
