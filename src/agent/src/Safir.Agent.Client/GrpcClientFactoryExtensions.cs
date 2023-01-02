using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client;

[PublicAPI]
public static class GrpcClientFactoryExtensions
{
    public static FileSystem.FileSystemClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
        => factory.CreateClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name));

    public static Host.HostClient CreateHostClient(this GrpcClientFactory factory, string name)
        => factory.CreateClient<Host.HostClient>(ClientName.Host(name));
}
