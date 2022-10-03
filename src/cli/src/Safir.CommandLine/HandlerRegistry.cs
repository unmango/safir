using System.CommandLine;
using System.CommandLine.Invocation;

namespace Safir.CommandLine;

internal sealed class HandlerRegistry
{
    private readonly Dictionary<Command, Lazy<HandlerContext>> _handlers;

    public HandlerRegistry(InvocationContext context, IEnumerable<KeyValuePair<Command, IHandlerBuilder>> handlers)
    {
        _handlers = handlers.ToDictionary(x => x.Key, x => new Lazy<HandlerContext>(() => x.Value.Build(context)));
    }

    public HandlerContext Get(Command command) => _handlers[command].Value;
}
