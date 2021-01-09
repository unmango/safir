using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Services;

namespace Cli.Internal
{
    internal static class InstallationServiceExtensions
    {
        public static Task InstallAsync(
            this IInstallationService installer,
            IEnumerable<ServiceEntry> services,
            bool concurrent = false,
            string? directory = null,
            CancellationToken cancellationToken = default)
            => concurrent
                ? Task.WhenAll(services.Select(x => installer.InstallAsync(x, directory, cancellationToken)))
                : InstallAsync(installer, services, directory, cancellationToken);

        public static async Task InstallAsync(
            this IInstallationService installer,
            IEnumerable<ServiceEntry> services,
            string? directory = null,
            CancellationToken cancellationToken = default)
        {
            foreach (var service in services)
            {
                await installer.InstallAsync(service, directory, cancellationToken);
            }
        }
    }
}
