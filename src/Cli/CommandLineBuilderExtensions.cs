using System;
using System.CommandLine;
using System.CommandLine.Builder;

namespace Cli
{
    internal static class CommandLineBuilderExtensions
    {
        public static T AddCommand<T>(this T builder, string name, Action<CommandBuilder> configure)
            where T : CommandLineBuilder
        {
            return builder.AddCommand(() => new Command(name), configure);
        }

        public static T AddCommand<T>(this T builder, string name, string description, Action<CommandBuilder> configure)
            where T : CommandLineBuilder
        {
            return builder.AddCommand(() => new Command(name, description), configure);
        }

        private static T AddCommand<T>(this T builder, Func<Command> factory, Action<CommandBuilder> configure)
            where T : CommandLineBuilder
        {
            var inner = new CommandBuilder(factory());

            configure(inner);

            return builder.AddCommand(inner.Command);
        }
    }
}
