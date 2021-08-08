using System.Threading;
using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public interface IHostClient
    {
        HostInfo GetInfo(CancellationToken cancellationToken = default);
        
        AsyncUnaryCall<HostInfo> GetInfoAsync(CancellationToken cancellationToken = default);
    }
}
