using Cli.Services.Installers;
using Cli.Services.Installers.Vcs;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceInstallationPipeline(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddTransient<IInstallationPipeline, DefaultInstallationPipeline>();

            services.AddLibGit2Sharp();
            services.AddTransient<IPipelineServiceInstaller, GitInstaller>();
            
            return services;
        }
    }
}
