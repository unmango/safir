using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Client.Internal;
using Safir.Agent.V1Alpha1;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Client.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
    {
        services.AddWrappers();
        services.AddGrpcClient<FilesService.FilesServiceClient>();
        services.AddGrpcClient<HostService.HostServiceClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddWrappers();
        services.AddGrpcClient<FilesService.FilesServiceClient>(configureClient);
        services.AddGrpcClient<HostService.HostServiceClient>(configureClient);

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<IServiceProvider, GrpcClientFactoryOptions> configureClient)
    {
        services.AddWrappers();
        services.AddGrpcClient<FilesService.FilesServiceClient>(configureClient);
        services.AddGrpcClient<HostService.HostServiceClient>(configureClient);

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services, string name)
    {
        services.AddWrappers();
        services.AddGrpcClient<FilesService.FilesServiceClient>(ClientName.FileSystem(name));
        services.AddGrpcClient<HostService.HostServiceClient>(ClientName.Host(name));

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        string name,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddWrappers();
        services.AddGrpcClient<FilesService.FilesServiceClient>(ClientName.FileSystem(name), configureClient);
        services.AddGrpcClient<HostService.HostServiceClient>(ClientName.Host(name), configureClient);

        return services;
    }

    private static void AddWrappers(this IServiceCollection services)
    {
        services.AddTransient<IFileSystemClient, FileSystemClientWrapper>();
        services.AddTransient<IHostClient, HostClientWrapper>();
    }
}
