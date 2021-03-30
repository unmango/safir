using System;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Safir.Agent.Protos;

namespace Safir.Agent.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirAgentClient(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddHttpClient();
            services.AddOptions();

            services.AddTransient<IFileSystemClient, DefaultFileSystemClient>();
            services.AddGrpcClient<FileSystem.FileSystemClient>(ConfigureGrpcClient);

            services.AddTransient<IHostClient, DefaultHostClient>();
            services.AddGrpcClient<Host.HostClient>(ConfigureGrpcClient);
            
            return services;
        }

        private static void ConfigureGrpcClient(IServiceProvider services, GrpcClientFactoryOptions options)
        {
            // TODO: Set service url
        }
    }
}
