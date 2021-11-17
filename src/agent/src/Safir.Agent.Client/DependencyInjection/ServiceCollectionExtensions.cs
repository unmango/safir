using System;
using Grpc.Net.ClientFactory;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Client.Internal;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Client.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
        {
            services.AddWrappers();
            services.AddGrpcClient<FileSystem.FileSystemClient>();
            services.AddGrpcClient<Host.HostClient>();

            return services;
        }
        
        public static IServiceCollection AddSafirAgentClient(
            this IServiceCollection services,
            Action<GrpcClientFactoryOptions> configureClient)
        {
            services.AddWrappers();
            services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);
            services.AddGrpcClient<Host.HostClient>(configureClient);

            return services;
        }
        
        public static IServiceCollection AddSafirAgentClient(
            this IServiceCollection services,
            Action<IServiceProvider, GrpcClientFactoryOptions> configureClient)
        {
            services.AddWrappers();
            services.AddGrpcClient<FileSystem.FileSystemClient>(configureClient);
            services.AddGrpcClient<Host.HostClient>(configureClient);

            return services;
        }
        
        public static IServiceCollection AddSafirAgentClient(this IServiceCollection services, string name)
        {
            services.AddWrappers();
            services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name));
            services.AddGrpcClient<Host.HostClient>(ClientName.Host(name));

            return services;
        }
        
        public static IServiceCollection AddSafirAgentClient(
            this IServiceCollection services,
            string name,
            Action<GrpcClientFactoryOptions> configureClient)
        {
            services.AddWrappers();
            services.AddGrpcClient<FileSystem.FileSystemClient>(ClientName.FileSystem(name), configureClient);
            services.AddGrpcClient<Host.HostClient>(ClientName.Host(name), configureClient);

            return services;
        }

        private static void AddWrappers(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemClient, FileSystemClientWrapper>();
            services.AddTransient<IHostClient, HostClientWrapper>();
        }
    }
}
