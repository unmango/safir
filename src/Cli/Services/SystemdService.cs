using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal class SystemdService : IService
    {
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
