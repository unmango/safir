using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation
{
    internal static class ServiceInstallerExtensions
    {
        public static Task InstallAsync(
            this IServiceInstaller installer,
            IEnumerable<IService> services,
            bool concurrent = false,
            string? directory = null,
            CancellationToken cancellationToken = default)
            => concurrent
                ? Task.WhenAll(services.Select(x => installer.InstallAsync(x, directory, cancellationToken)))
                : InstallAsync(installer, services, directory, cancellationToken);

        public static async Task InstallAsync(
            this IServiceInstaller installer,
            IEnumerable<IService> services,
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
