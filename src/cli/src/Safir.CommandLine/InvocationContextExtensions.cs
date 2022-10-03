using System.CommandLine;
using System.CommandLine.Invocation;
using JetBrains.Annotations;

namespace Safir.CommandLine;

[PublicAPI]
public static class InvocationContextExtensions
{
    public static HandlerContext GetHandlerContext(this InvocationContext context, Command command)
        => command.GetHandlerContext(context);
}
