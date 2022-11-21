using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
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
    private static readonly Lazy<Mock<GrpcClientFactory>> _grpcClientFactoryMock = new(() => new());

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
        where TClient : class
        => factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services => {
            _grpcClientFactoryMock.Value.Setup(x => x.CreateClient<TClient>(It.IsAny<string>())).Returns(client);

            var descriptor = services.FirstOrDefault(x => x.ImplementationInstance == _grpcClientFactoryMock.Value.Object);
            if (descriptor is null)
                services.AddSingleton(_grpcClientFactoryMock.Value.Object);
        }));

    public static WebApplicationFactory<T> WithGrpcClient<T, TClient>(this WebApplicationFactory<T> factory, Mock<TClient> client)
        where T : class
        where TClient : class
        => factory.WithGrpcClient(client.Object);

    public static WebApplicationFactory<T> WithOptions<T, TOptions>(this WebApplicationFactory<T> factory, Func<TOptions> options)
        where T : class
        where TOptions : class
        => factory.WithWebHostBuilder(builder => {
            builder.ConfigureTestServices(services => {
                var mock = new Mock<IOptions<TOptions>>();
                mock.SetupGet(x => x.Value).Returns(options);
                services.AddSingleton(mock.Object);
            });
        });

    public static WebApplicationFactory<T> WithOptions<T, TOptions>(this WebApplicationFactory<T> factory, TOptions options)
        where T : class
        where TOptions : class
        => factory.WithOptions(() => options);
}
