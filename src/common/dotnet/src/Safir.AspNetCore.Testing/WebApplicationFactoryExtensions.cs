using Grpc.Net.Client;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Safir.AspNetCore.Testing;

[PublicAPI]
public static class WebApplicationFactoryExtensions
{
    public static GrpcChannel CreateChannel<T>(this WebApplicationFactory<T> factory)
        where T : class
        => GrpcChannel.ForAddress("http://localhost", new() {
            HttpHandler = factory.Server.CreateHandler(),
        });

    public static WebApplicationFactory<T> WithConfiguration<T>(
        this WebApplicationFactory<T> factory,
        Action<IConfigurationBuilder> configure)
        where T : class
        => factory.WithWebHostBuilder(builder => builder.ConfigureAppConfiguration(configure));

    public static WebApplicationFactory<T> WithGrpcClient<T, TClient>(this WebApplicationFactory<T> factory, TClient client)
        where T : class
        => factory.WithWebHostBuilder(builder => builder.ConfigureServices((context, services) => {
            // TODO: Was thinking about how to mock GrpcClientFactory w/o overwriting on multiple calls to WithGrpcClient
        }));

    public static WebApplicationFactory<T> WithOptions<T, TOptions>(this WebApplicationFactory<T> factory, TOptions options)
        where T : class
        where TOptions : class
        => factory.WithWebHostBuilder(builder => {
            builder.ConfigureTestServices(services => {
                var mock = new Mock<IOptions<TOptions>>();
                mock.SetupGet(x => x.Value).Returns(options);
                services.AddSingleton(mock.Object);
            });
        });
}
