using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

namespace Cli.Middleware
{
    internal static class HelpMiddleware
    {
        public static CommandLineBuilder UseHelpForEmptyCommands(this CommandLineBuilder builder) =>
            builder.UseMiddleware((context, next) => {
                var globalOptions = ((Command)context.ParseResult.RootCommandResult.Command).GlobalOptions;
                var commandResult = context.ParseResult.CommandResult;
                
                if (HasChildrenDefined(commandResult.Command, globalOptions) &&
                    !WasPassedChildren(commandResult))
                {
                    return CommandHandler.Create((IHelpBuilder help) => {
                        help.Write(commandResult.Command);
                    }).InvokeAsync(context);
                }

                return next(context);
            });

        private static bool HasChildrenDefined(ISymbol command, IEnumerable<IOption> globalOptions)
        {
            return command.Children.Except(globalOptions).Any();
        }

        private static bool WasPassedChildren(SymbolResult result)
        {
            return result.Children.Count > 0;
        }
    }
}
