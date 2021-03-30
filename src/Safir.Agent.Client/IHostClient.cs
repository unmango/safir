using System.Threading;
using System.Threading.Tasks;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    public interface IHostClient
    {
        Task<HostInfo> GetHostInfoAsync(CancellationToken cancellationToken = default);
    }
}
