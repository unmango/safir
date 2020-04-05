using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers an action to configure commands.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T ConfigureCommands<T>(this T builder, Action<CommandLineBuilder> configure)
            where T : IApplicationBuilder
        {
            builder.ConfigureCommands((_, commandLineBuilder) => configure(commandLineBuilder));

            return builder;
        }

        /// <summary>
        /// Registers an action to configure app configuration.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T ConfigureConfiguration<T>(this T builder, Action<IConfigurationBuilder> configure)
            where T : IApplicationBuilder
        {
            builder.ConfigureConfiguration((_, configurationBuilder) => configure(configurationBuilder));

            return builder;
        }

        /// <summary>
        /// Registers an action to configure app services.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T ConfigureServices<T>(this T builder, Action<IServiceCollection> configure)
            where T : IApplicationBuilder
        {
            builder.ConfigureServices((_, services) => configure(services));

            return builder;
        }
    }
}
