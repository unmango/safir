using System.CommandLine;
using System.CommandLine.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.Cli.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafirCliCore(this IServiceCollection services)
    {
        services.AddSingleton<IConsole, SystemConsole>();
        // services.Configure<GlobalOptions>(Application.Configuration);
        services.AddLogging();

        return services;
    }

    public static ServiceProviderBinderFactory BuildBinderFactory(this IServiceCollection services) => new(services);
}
