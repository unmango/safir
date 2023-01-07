using JetBrains.Annotations;
using Safir.Agent.V1Alpha1;
using Safir.Common.V1Alpha1;

namespace Safir.Manager;

[PublicAPI]
public interface IAgents
{
    IEnumerable<KeyValuePair<string, FilesService.FilesServiceClient>> FileSystem { get; }

    IEnumerable<KeyValuePair<string, HostService.HostServiceClient>> Host { get; }
}
