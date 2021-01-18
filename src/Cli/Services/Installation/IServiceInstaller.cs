using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation
{
    internal interface IServiceInstaller
    {
        ValueTask<ServiceInstalled> GetInstalledAsync(IService service, CancellationToken cancellationToken = default);

        Task InstallAsync(IService service, string? directory = null, CancellationToken cancellationToken = default);
    }
}
