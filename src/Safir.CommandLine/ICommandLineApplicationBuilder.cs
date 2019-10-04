using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    public interface ICommandLineApplicationBuilder
    {
        ICommandLineApplication Build();

        ICommandLineApplicationBuilder ConfigureConfiguration(Action<CommandLineApplicationBuilderContext, IConfigurationBuilder> configure);

        ICommandLineApplicationBuilder ConfigureServices(Action<CommandLineApplicationBuilderContext, IServiceCollection> configure);

        ICommandLineApplicationBuilder UseServiceProviderFactory<T>(Func<CommandLineApplicationBuilderContext, IServiceProviderFactory<T>> factory);
    }
}
