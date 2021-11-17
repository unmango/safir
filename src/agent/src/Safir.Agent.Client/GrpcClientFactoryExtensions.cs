using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public static class GrpcClientFactoryExtensions
    {
        public static IFileSystemClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
        {
            var clientName = ClientName.FileSystem(name);
            var client = factory.CreateClient<FileSystem.FileSystemClient>(clientName);
            return new FileSystemClientWrapper(client);
        }

        public static IHostClient CreateHostClient(this GrpcClientFactory factory, string name)
        {
            var clientName = ClientName.Host(name);
            var client = factory.CreateClient<Host.HostClient>(clientName);
            return new HostClientWrapper(client);
        }
    }
}
