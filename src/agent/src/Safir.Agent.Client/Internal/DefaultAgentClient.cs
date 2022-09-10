using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client.Internal;

internal sealed class DefaultAgentClient : IAgentClient
{
    public DefaultAgentClient(FileSystem.FileSystemClient fileSystem, Host.HostClient host)
    {
        FileSystem = fileSystem;
        Host = host;
    }

    public FileSystem.FileSystemClient FileSystem { get; }

    public Host.HostClient Host { get; }
}
