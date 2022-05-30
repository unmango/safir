using System;
using JetBrains.Annotations;

namespace Safir.CommandLine;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public class CommandHandlerAttribute : Attribute
{
}
