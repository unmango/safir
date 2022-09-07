using JetBrains.Annotations;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client;

[PublicAPI]
public interface IAgentClient
{
    FileSystem.FileSystemClient FileSystem { get; }

    Host.HostClient Host { get; }
}
