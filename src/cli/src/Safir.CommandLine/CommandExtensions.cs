using System.CommandLine;
using JetBrains.Annotations;

namespace Safir.CommandLine;

[PublicAPI]
public static class CommandExtensions
{
    public static void SetHandler(this Command command, IHandlerBuilder builder) => command.Handler = builder.Build();
}
