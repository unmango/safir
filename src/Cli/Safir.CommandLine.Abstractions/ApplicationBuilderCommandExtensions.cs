using System;
using System.CommandLine;
using System.CommandLine.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine
{
    /// <summary>
    /// Extensions for configuring commands on an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderCommandExtensions
    {
        public static TBuilder AddCommand<TBuilder>(this TBuilder builder, Command command, Type? commandType = null)
            where TBuilder : IApplicationBuilder
        {
            return builder
                .ConfigureCommands(commandBuilder => commandBuilder.AddCommand(command))
                .ConfigureServices(services => services.AddScoped(commandType ?? command.GetType()));
        }

        public static TBuilder AddCommand<TBuilder, TCommand>(this TBuilder builder, TCommand command)
            where TBuilder : IApplicationBuilder
            where TCommand : Command
        {
            return builder.AddCommand(command, typeof(TCommand));
        }

        public static IApplicationBuilder AddCommand<T>(this IApplicationBuilder builder, T command)
            where T : Command
        {
            return builder.AddCommand(command, typeof(T));
        }

        public static T AddCommand<T>(this T builder, string name, Action<CommandBuilder>? configure = null)
            where T : IApplicationBuilder
            => builder.AddCommand(name, null, configure);

        public static T AddCommand<T>(this T builder, string name, string? description, Action<CommandBuilder>? configure = null)
            where T : IApplicationBuilder
        {
            var command = new Command(name, description);

            configure?.Invoke(new CommandBuilder(command));

            return builder.AddCommand(command);
        }
    }
}
