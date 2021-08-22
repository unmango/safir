using System;
using Safir.Agent.Client;

namespace Safir.Manager.Agents
{
    internal class AgentClient : IAgent
    {
        public AgentClient(string name, IFileSystemClient fileSystem, IHostClient host)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IFileSystemClient FileSystem { get; }

        public IHostClient Host { get; }

        public string Name { get; }
    }
}
