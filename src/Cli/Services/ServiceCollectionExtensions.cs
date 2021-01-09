using Cli.Services.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceInstallationPipeline(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddTransient<IInstallationPipeline, DefaultInstallationPipeline>();
            services.AddTransient<IPipelineServiceInstaller, GitInstaller>();
            
            return services;
        }
    }
}
