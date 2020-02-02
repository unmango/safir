using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace System.CommandLine
{
    public static class CommandBuilderExtensions
    {
        //public static T UseHandler<T>(this T builder, ICommandHandler handler)
        //    where T : CommandBuilder
        //{
        //    builder.Command.Handler = handler;

        //    return builder;
        //}

        //public static T AddCommand<T>(this T builder, string name, Action<CommandBuilder> configure)
        //    where T : CommandBuilder
        //    => builder.AddCommand(new Command(name), configure);

        //public static T AddCommand<T>(this T builder, string name, string description, Action<CommandBuilder> configure)
        //    where T : CommandBuilder
        //    => builder.AddCommand(new Command(name, description), configure);

        //private static T AddCommand<T>(this T builder, Command command, Action<CommandBuilder> configure)
        //    where T : CommandBuilder
        //{
        //    configure(new CommandBuilder(command));

        //    return builder.AddCommand(command);
        //}
    }
}
