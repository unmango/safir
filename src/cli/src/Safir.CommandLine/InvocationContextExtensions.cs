using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Safir.CommandLine;

[PublicAPI]
public static class InvocationContextExtensions
{
    public static bool TryGetHandlerContext(
        this InvocationContext context,
        [MaybeNullWhen(false)] out HandlerContext handlerContext)
    {
        var command = context.ParseResult.CommandResult.Command;

        handlerContext = command.Handler switch {
            HandlerApplication app => app.GetContext(context),
            _ => null,
        };

        return handlerContext is not null;
    }
}
