using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Services.Installers.Vcs
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLibGit2Sharp(this IServiceCollection services)
        {
            // TODO: Is this valid
            services.AddTransient<IRepository, Repository>();
            
            // Wrappers
            services.AddTransient<IRepositoryFunctions, LibGit2SharpStaticRepositoryWrapper>();
            services.AddTransient<IRemoteFunctions, LibGit2SharpStaticRemoteWrapper>();
            
            return services;
        }
    }
}
