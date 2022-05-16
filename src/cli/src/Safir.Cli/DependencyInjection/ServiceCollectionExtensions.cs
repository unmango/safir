using System.CommandLine;
using System.CommandLine.Binding;
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

    public static BinderBase<T> CreateBinder<T>(this IServiceCollection services)
        where T : notnull
        => new ServiceProviderBinder<T>(services);

    public static IBinderFactory BuildBinderFactory(this IServiceCollection services)
        => new ServiceProviderBinderFactory(services);

    private sealed class ServiceProviderBinder<T> : BinderBase<T>
        where T : notnull
    {
        private readonly IServiceCollection _services;

        public ServiceProviderBinder(IServiceCollection services)
        {
            _services = services;
        }

        protected override T GetBoundValue(BindingContext bindingContext)
        {
            return _services.BuildServiceProvider().GetRequiredService<T>();
        }
    }
}
