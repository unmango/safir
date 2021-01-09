using Cli.Internal.Wrappers.Git;
using Cli.Services.Installation.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Services.Installation
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceInstallationPipeline(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddTransient<IInstallationPipeline, DefaultInstallationPipeline>();

            services.AddLibGit2Sharp();
            services.AddTransient<IInstallationMiddleware, GitInstaller>();
            
            return services;
        }
        
        public static IServiceCollection AddInstallationService(this IServiceCollection services)
        {
            services.AddServiceInstallationPipeline();
            services.AddTransient<IInstallationService, PipelineInstallationService>();
            
            return services;
        }
    }
}
