using Cli.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Internal
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInstallationService(this IServiceCollection services)
        {
            services.AddServiceInstallationPipeline();
            services.AddTransient<IInstallationService, PipelineInstallationService>();
            
            return services;
        }
    }
}
