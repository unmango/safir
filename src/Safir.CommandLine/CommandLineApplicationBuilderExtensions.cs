using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    public static class CommandLineApplicationBuilderExtensions
    {
        public static ICommandLineApplicationBuilder UseServiceProviderFactory<T>(
            this ICommandLineApplicationBuilder builder,
            IServiceProviderFactory<T> factory)
        {
            return builder.UseServiceProviderFactory(_ => factory);
        }
    }
}
