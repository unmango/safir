using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.CommandLine;

public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseHandlerBuilders(
        this CommandLineBuilder builder,
        IEnumerable<KeyValuePair<Command, IHandlerBuilder>> handlers)
        => builder.AddMiddleware(context => {
            var map = handlers.ToDictionary(x => x.Key, x => x.Value);
            var registry = new HandlerRegistry(context, map);
            var command = context.ParseResult.CommandResult.Command;
            context.BindingContext.AddService(_ => registry);
            context.BindingContext.AddService(_ => registry.Get(command));
        });
}
