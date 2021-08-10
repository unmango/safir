using System;
using Grpc.Net.ClientFactory;

namespace Safir.Manager.Agents
{
    internal class AgentFactory
    {
        private readonly GrpcClientFactory _factory;

        public AgentFactory(GrpcClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public IAgent Create(string name)
        {
            return new AgentProxy(name, _factory);
        }
    }
}
