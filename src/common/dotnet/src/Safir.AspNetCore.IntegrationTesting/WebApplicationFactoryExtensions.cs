using Grpc.Net.Client;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Safir.AspNetCore.IntegrationTesting;

[PublicAPI]
public static class WebApplicationFactoryExtensions
{
    public static GrpcChannel CreateChannel<T>(this WebApplicationFactory<T> factory)
        where T : class
        => GrpcChannel.ForAddress("http://localhost", new() {
            HttpHandler = factory.Server.CreateHandler(),
        });
}
