using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Grpc.Client;
using Safir.Grpc.Client.DependencyInjection;
using Safir.Protos;

namespace Safir.Agent.Client.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
    {
        services.AddSafirAgentClientCore();
        services.AddGrpcClient<FileSystem.FileSystemClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddSafirAgentClientCore();
        services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<IServiceProvider, GrpcClientFactoryOptions> configureClient)
    {
        services.AddSafirGrpcHostClient();
        services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services, string name)
    {
        services.AddSafirAgentClientCore();
        services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name));

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        string name,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddSafirAgentClientCore();
        services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name), configureClient);

        return services;
    }

    private static void AddSafirAgentClientCore(this IServiceCollection services)
    {
        services.AddSafirGrpcHostClient();
        services.AddTransient<IAgentClient, DefaultAgentClient>();
    }
}
