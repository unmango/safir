using JetBrains.Annotations;
using Safir.Protos;

namespace Safir.Grpc.Client.Abstractions;

[PublicAPI]
public interface ISafirService
{
    Host.HostClient Host { get; }
}
