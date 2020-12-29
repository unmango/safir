using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal static class ServiceSourceExtensions
    {
        public static async Task TryInitializeAsync(
            this IServiceSource source,
            ServiceEntry service,
            string workingDirectory,
            CancellationToken cancellationToken)
        {
            var canInitialize = await source.CanInitializeAsync(cancellationToken);

            if (canInitialize.Failed)
                throw new NotSupportedException(
                    $"Can't initialize source {source.GetType()}: {canInitialize.FailureMessage}");

            if (!await source.IsInitializedAsync(service, workingDirectory, cancellationToken))
                await source.InitializeAsync(service, workingDirectory, cancellationToken);
        }
    }
}
