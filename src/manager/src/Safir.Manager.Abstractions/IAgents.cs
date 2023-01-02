using JetBrains.Annotations;
using Safir.Agent.Protos;
using Host = Safir.Protos.Host;

namespace Safir.Manager;

[PublicAPI]
public interface IAgents
{
    IEnumerable<KeyValuePair<string, FileSystem.FileSystemClient>> FileSystem { get; }

    IEnumerable<KeyValuePair<string, Host.HostClient>> Host { get; }
}
