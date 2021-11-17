using System;
using Grpc.Net.ClientFactory;
using Safir.Agent.Client;

namespace Safir.Manager.Agents
{
    internal class AgentFactory
    {
        private readonly GrpcClientFactory _factory;

        public AgentFactory(GrpcClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public virtual IAgent Create(string name)
        {
            var fileSystem = _factory.CreateFileSystemClient(name);
            var host = _factory.CreateHostClient(name);
            
            return new AgentClient(name, fileSystem, host);
        }
    }
}
