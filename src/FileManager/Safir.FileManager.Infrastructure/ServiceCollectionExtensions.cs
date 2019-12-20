using System;
using Microsoft.Extensions.DependencyInjection;
using Safir.FileManager.Infrastructure.Configuration;
using Safir.FileManager.Infrastructure.Data;
using Safir.FileManager.Infrastructure.Repositories;

namespace Safir.FileManager.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManagerInfrastructure(this IServiceCollection services)
            => services.AddFileManagerInfrastructure(_ => { });

        public static IServiceCollection AddFileManagerInfrastructure(
            this IServiceCollection services,
            Action<FileManagerOptions> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure(configure);
            services.AddConfiguration();

            services.AddDbContext<FileContext>();
            services.AddRepositories();

            return services;
        }
    }
}
