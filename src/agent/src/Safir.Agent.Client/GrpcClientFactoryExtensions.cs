using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.V1Alpha1;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public static class GrpcClientFactoryExtensions
{
    public static FilesService.FilesServiceClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
        => factory.CreateClient<FilesService.FilesServiceClient>(ClientName.FileSystem(name));

    public static HostService.HostServiceClient CreateHostClient(this GrpcClientFactory factory, string name)
        => factory.CreateClient<HostService.HostServiceClient>(ClientName.Host(name));
}
