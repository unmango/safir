using Microsoft.Extensions.DependencyInjection;

namespace Safir.FileManager.Infrastructure.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddConfiguration(this IServiceCollection services)
        {
            services.ConfigureOptions<DefaultDbConnectionConfiguration>();
        }
    }
}
