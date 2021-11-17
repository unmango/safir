using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation
{
    internal interface IInstallationService
    {
        Task InstallAsync(
            IService service,
            string? directory = null,
            CancellationToken cancellationToken = default);
    }
}
