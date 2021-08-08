using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public static class GrpcClientFactoryExtensions
    {
        public static IFileSystemClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
        {
            return new FileSystemClientWrapper(factory.CreateClient<FileSystem.FileSystemClient>(name));
        }

        public static IHostClient CreateHostClient(this GrpcClientFactory factory, string name)
        {
            return new HostClientWrapper(factory.CreateClient<Host.HostClient>(name));
        }
    }
}
