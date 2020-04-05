using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System.CommandLine
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureLogging(this IApplicationBuilder builder, Action<ILoggingBuilder> configure)
            => builder.ConfigureLogging((_, loggingBuilder) => configure(loggingBuilder));

        public static IApplicationBuilder ConfigureLogging(
            this IApplicationBuilder builder,
            Action<ApplicationBuilderContext, ILoggingBuilder> configure)
            => builder.ConfigureServices(
                (context, services) => services.AddLogging(
                    loggingBuilder => configure(context, loggingBuilder)));
    }
}
