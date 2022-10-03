using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.CommandLine;

public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseHandlerBuilders(
        this CommandLineBuilder builder,
        IEnumerable<KeyValuePair<Command, IHandlerBuilder>> handlers)
        => builder.AddMiddleware(context => {
            var registry = new HandlerRegistry(context, handlers);
            context.BindingContext.AddService(_ => registry);
        });
}
