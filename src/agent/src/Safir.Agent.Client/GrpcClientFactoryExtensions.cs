using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Safir.Agent.Client.Internal;
using Safir.Agent.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public static class GrpcClientFactoryExtensions
{
    public static FilesService.FilesServiceClient CreateFileSystemClient(this GrpcClientFactory factory, string name)
        => factory.CreateClient<FilesService.FilesServiceClient>(ClientName.FileSystem(name));
}
