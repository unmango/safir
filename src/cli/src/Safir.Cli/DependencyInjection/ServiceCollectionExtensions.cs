using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.Configuration;

namespace Safir.Cli.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafirCliCore(this IServiceCollection services) => services
        .AddSingleton<IConsole, SystemConsole>()
        .AddLogging();

    public static IServiceCollection AddIoAbstractions(this IServiceCollection services) => services
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<IFile>(s => s.GetRequiredService<IFileSystem>().File)
        .AddSingleton<IDirectory>(s => s.GetRequiredService<IFileSystem>().Directory)
        .AddSingleton<IPath>(s => s.GetRequiredService<IFileSystem>().Path);

    public static IServiceCollection AddLocalConfiguration(this IServiceCollection services) => services
        .AddLogging()
        .AddOptions()
        .AddIoAbstractions()
        .AddSingleton<IUserConfiguration, JsonUserConfiguration>();

    public static IServiceCollection AddSafirOptions(this IServiceCollection services) => services
        .AddOptions<SafirOptions>().BindConfiguration(configSectionPath: string.Empty)
        .Services;

    public static BinderBase<T> CreateBinder<T>(this IServiceCollection services)
        where T : notnull
        => new ServiceProviderBinder<T>(services);

    private sealed class ServiceProviderBinder<T> : BinderBase<T>
        where T : notnull
    {
        private readonly Lazy<IServiceProvider> _services;

        public ServiceProviderBinder(IServiceCollection services)
        {
            _services = new(services.BuildServiceProvider);
        }

        protected override T GetBoundValue(BindingContext bindingContext)
        {
            return _services.Value.GetRequiredService<T>();
        }
    }
}
