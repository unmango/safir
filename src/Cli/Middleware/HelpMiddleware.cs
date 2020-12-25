using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

namespace Cli.Middleware
{
    internal static class HelpMiddleware
    {
        public static CommandLineBuilder UseHelpForEmptyCommands(this CommandLineBuilder builder) =>
            builder.UseMiddleware((context, next) => {
                if (context.ParseResult.CommandResult.Children.Count > 0)
                {
                    return next(context);
                }

                return CommandHandler.Create((IHelpBuilder help) => {
                    help.Write(context.ParseResult.CommandResult.Command);
                }).InvokeAsync(context);
            });
    }
}
