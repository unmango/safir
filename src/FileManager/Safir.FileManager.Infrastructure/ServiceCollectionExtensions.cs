using Microsoft.EntityFrameworkCore;
using Safir.FileManager.Infrastructure;
using Safir.FileManager.Infrastructure.Data;
using System;

namespace Microsoft.Extensions.DependencyInjection
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

            services.AddEventInfrastructure();
            services.AddRepositories();

            return services;
        }
    }
}
