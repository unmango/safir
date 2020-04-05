using System;
using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.CommandLine
{
    /// <summary>
    /// Extensions for configuring commands on an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderCommandExtensions
    {
        public static T AddCommand<T>(this T builder, Command command)
            where T : IApplicationBuilder
        {
            return builder.ConfigureCommands(commandBuilder => commandBuilder.AddCommand(command));
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
