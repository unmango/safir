using System.Collections.Generic;
using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    /// <summary>
    /// Used for building <see cref="ICommandLineApplication"/>s.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets the properties of the <see cref="IApplicationBuilder"/>.
        /// </summary>
        IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Builds an <see cref="ICommandLineApplication"/>.
        /// </summary>
        /// <returns>The built <see cref="ICommandLineApplication"/>.</returns>
        ICommandLineApplication Build();

        /// <summary>
        /// Registers an action to configure application commands.
        /// </summary>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        IApplicationBuilder ConfigureCommands(Action<ApplicationBuilderContext, CommandLineBuilder> configure);

        /// <summary>
        /// Registers an action to configure application configuration.
        /// </summary>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        IApplicationBuilder ConfigureConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> configure);

        /// <summary>
        /// Registers an action to configure application services.
        /// </summary>
        /// <param name="configure">The action to configure with.</param>
        /// <returns>The builder so calls can be chained.</returns>
        IApplicationBuilder ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configure);
    }
}
