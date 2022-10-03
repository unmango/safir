using System.CommandLine;
using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class CommandExtensions
{
    private const string NoRegistryMessage =
        $"Unable to get handler context. Has {nameof(CommandLineBuilderExtensions.UseHandlerBuilders)} been called?";

    public static HandlerContext GetHandlerContext(this Command command, InvocationContext context)
    {
        var registry = context.BindingContext.GetService<HandlerRegistry>()
                       ?? throw new InvalidOperationException(NoRegistryMessage);

        return registry.Get(command);
    }
}
