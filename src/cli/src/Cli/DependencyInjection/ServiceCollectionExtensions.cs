using Cli.Internal;
using Cli.Internal.Progress;
using Cli.Internal.Wrappers.Git;
using Cli.Services;
using Cli.Services.Installation;
using Cli.Services.Installation.Installers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirCliServices(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();

            services.AddScoped<IProgressReporter, ConsoleProgressReporter>();
            
            services.AddTransient<IServiceDirectory, ConfigurationServiceDirectory>();
            services.AddScoped<IServiceRegistry, DefaultServiceRegistry>();
            
            services.AddTransient<IInstallationPipeline, DefaultInstallationPipeline>();
            services.AddTransient<IInstallationService, PipelineInstallationService>();

            services.AddLibGit2Sharp();
            services.AddTransient<IInstallationMiddleware, GitInstaller>();
            
            return services;
        }
    }
}
