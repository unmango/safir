using System;
using System.Threading;
using Grpc.Core;
using Safir.Protos;

namespace Safir.Agent.Client.Internal
{
    internal class HostClientWrapper : IHostClient
    {
        private readonly Host.HostClient _client;

        public HostClientWrapper(Host.HostClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HostInfo GetInfo(CancellationToken cancellationToken = default)
        {
            return _client.GetInfo(cancellationToken);
        }

        public AsyncUnaryCall<HostInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetInfoAsync(cancellationToken);
        }
    }
}
