using System;
using Safir.Files.Manager;
using Safir.Files.Manager.Client;
using Safir.Files.Manager.Client.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManagerClient(this IServiceCollection services)
        {
            services.AddOptions<FileManagerOptions>();
            services.AddHttpClient(FileManagerOptions.ClientName);
            services.AddGrpcClient<Greeter.GreeterClient>(FileManagerOptions.ClientName);

            services.AddTransient<IFileManagerClient, DefaultFileManagerClient>();

            return services;
        }

        public static IServiceCollection AddFileManagerClient(
            this IServiceCollection services,
            Action<FileManagerOptions> configure)
        {
            services.Configure(configure);

            return services.AddFileManagerClient();
        }
    }
}
