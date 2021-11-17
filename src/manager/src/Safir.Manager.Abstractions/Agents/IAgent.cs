using JetBrains.Annotations;
using Safir.Agent.Client;

namespace Safir.Manager.Agents
{
    [PublicAPI]
    public interface IAgent
    {
        IFileSystemClient FileSystem { get; }
        
        IHostClient Host { get; }
        
        string Name { get; }
    }
}
