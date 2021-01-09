using System.Threading;
using System.Threading.Tasks;
using Cli.Services;

namespace Cli.Internal
{
    internal interface IInstallationService
    {
        Task InstallAsync(
            ServiceEntry service,
            string? directory = null,
            CancellationToken cancellationToken = default);
    }
}
