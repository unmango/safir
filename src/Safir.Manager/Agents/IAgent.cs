using Safir.Agent.Client;

namespace Safir.Manager.Agents
{
    internal interface IAgent
    {
        IFileSystemClient FileSystem { get; }
        
        IHostClient Host { get; }
        
        string Name { get; }
    }
}
