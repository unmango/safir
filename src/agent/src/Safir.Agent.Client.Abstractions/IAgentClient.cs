using JetBrains.Annotations;
using Safir.Agent.Protos;
using Safir.Grpc.Client.Abstractions;

namespace Safir.Agent.Client;

[PublicAPI]
public interface IAgentClient : ISafirService
{
    FileSystem.FileSystemClient FileSystem { get; }
}
