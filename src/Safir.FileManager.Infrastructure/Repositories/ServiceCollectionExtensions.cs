using Microsoft.Extensions.DependencyInjection;
using Safir.FileManager.Domain.Repositories;

namespace Safir.FileManager.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManagerRepositories(this IServiceCollection services)
        {
            services.AddRepositories();

            return services;
        }

        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
        }
    }
}
