using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
    {
        services.AddGrpcClient<FileSystem.FileSystemClient>();
        services.AddGrpcClient<Host.HostClient>();
        services.AddTransient<IAgentClient, DefaultAgentClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);
        services.AddGrpcClient<Host.HostClient>(configureClient);
        services.AddTransient<IAgentClient, DefaultAgentClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        Action<IServiceProvider, GrpcClientFactoryOptions> configureClient)
    {
        services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);
        services.AddGrpcClient<Host.HostClient>(configureClient);
        services.AddTransient<IAgentClient, DefaultAgentClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(this IServiceCollection services, string name)
    {
        services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name));
        services.AddGrpcClient<Host.HostClient>(ClientName.Host(name));
        services.AddTransient<IAgentClient, DefaultAgentClient>();

        return services;
    }

    public static IServiceCollection AddSafirAgentClient(
        this IServiceCollection services,
        string name,
        Action<GrpcClientFactoryOptions> configureClient)
    {
        services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name), configureClient);
        services.AddGrpcClient<Host.HostClient>(ClientName.Host(name), configureClient);
        services.AddTransient<IAgentClient, DefaultAgentClient>();

        return services;
    }
}
