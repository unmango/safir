using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Safir.FileManager.Infrastructure.Data;
using Safir.FileManager.Infrastructure.Repositories;
using System;

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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure(configure);

            services.AddDbContext<FileContext>(options =>
            {
                options.UseSqlite("");
            });

            services.AddRepositories();

            return services;
        }
    }
}
