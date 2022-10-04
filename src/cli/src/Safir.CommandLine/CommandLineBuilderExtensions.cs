using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.CommandLine;

public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseCommandHandlers(
        this CommandLineBuilder builder,
        IEnumerable<KeyValuePair<Command, IHandlerBuilder>> handlers)
        => builder.AddMiddleware(context => {
            var command = context.ParseResult.CommandResult.Command;
            var map = handlers.ToDictionary(x => x.Key, x => x.Value);

            if (!map.TryGetValue(command, out var handlerBuilder))
                return;

            var handlerContext = handlerBuilder.Build(context);
            context.BindingContext.AddService(_ => handlerContext);
        });
}
