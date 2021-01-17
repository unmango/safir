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
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default);

        ValueTask<IServiceUpdate> GetUpdateAsync(
            T source,
            InstallationContext context,
            CancellationToken cancellationToken = default);

        ValueTask InstallAsync(T source, InstallationContext context, CancellationToken cancellationToken = default);

        ValueTask UpdateAsync(T source, InstallationContext context, CancellationToken cancellationToken = default);
    }
}
