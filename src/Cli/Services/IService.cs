using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal interface IService
    {
        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
