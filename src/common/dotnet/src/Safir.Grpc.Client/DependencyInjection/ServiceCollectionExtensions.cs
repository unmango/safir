using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Safir.Grpc.Client.Abstractions;
using Safir.Protos;

namespace Safir.Grpc.Client.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSafirGrpcHostClient(this IServiceCollection services)
        => services
            .AddSafirService()
            .AddGrpcClient<Host.HostClient>();

    public static IHttpClientBuilder AddSafirGrpcHostClient(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configureClient)
        => services
            .AddSafirService()
            .AddGrpcClient<Host.HostClient>(configureClient);

    public static IHttpClientBuilder AddSafirGrpcHostClient(
        this IServiceCollection services,
        Action<IServiceProvider, GrpcClientFactoryOptions> configureClient)
        => services
            .AddSafirService()
            .AddGrpcClient<Host.HostClient>(configureClient);

    public static IHttpClientBuilder AddSafirGrpcHostClient(this IServiceCollection services, string name)
        => services
            .AddSafirService()
            .AddGrpcClient<Host.HostClient>(ClientName.Host(name));

    public static IHttpClientBuilder AddSafirGrpcHostClient(
        this IServiceCollection services,
        string name,
        Action<GrpcClientFactoryOptions> configureClient)
        => services
            .AddSafirService()
            .AddGrpcClient<Host.HostClient>(ClientName.Host(name), configureClient);

    private static IServiceCollection AddSafirService(this IServiceCollection services)
    {
        services.TryAddTransient<ISafirService, DefaultSafirService>();
        return services;
    }

    private record DefaultSafirService(Host.HostClient Host) : ISafirService;
}
