using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Grpc.Client;
using Safir.Protos;

namespace Safir.Agent.Client;

// TODO: This name templating is going to bite me in the ass
//       one day, but ADHD needs me to commit things

[PublicAPI]
public static class GrpcClientFactoryExtensions
{
    public static IAgentClient CreateAgentClient(this GrpcClientFactory clientFactory, string name)
    {
        return new DefaultAgentClient(
            clientFactory.CreateFileSystemClient(name),
            clientFactory.CreateHostClient(name));
    }

    public static FileSystem.FileSystemClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
    {
        var clientName = ClientName.FileSystem(name);
        return factory.CreateClient<FileSystem.FileSystemClient>(clientName);
    }

    public static Host.HostClient CreateHostClient(this GrpcClientFactory factory, string name)
    {
        var clientName = ClientName.Host(name);
        return factory.CreateClient<Host.HostClient>(clientName);
    }
}
