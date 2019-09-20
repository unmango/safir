using Safir.FileManager.Domain.Repositories;
using Safir.FileManager.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryServiceCollectionExtensions
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
