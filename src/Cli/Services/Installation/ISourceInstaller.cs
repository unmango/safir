using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal interface ISourceInstaller : IAppliesTo<ISourceContext>
    {
    }

    internal interface ISourceInstaller<T> : ISourceInstaller
        where T : IServiceSource
    {
        ValueTask<SourceInstalled> GetInstalledAsync(
            SourceContext<T> context,
            CancellationToken cancellationToken = default);

        ValueTask<ServiceUpdate> GetUpdateAsync(
            SourceContext<T> context,
            CancellationToken cancellationToken = default);

        ValueTask InstallAsync(SourceContext<T> context, CancellationToken cancellationToken = default);

        ValueTask UpdateAsync(SourceContext<T> context, CancellationToken cancellationToken = default);
    }
}
