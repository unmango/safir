using JetBrains.Annotations;

namespace Safir.CommandLine.Generator;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public class CommandHandlerAttribute : Attribute
{
}
