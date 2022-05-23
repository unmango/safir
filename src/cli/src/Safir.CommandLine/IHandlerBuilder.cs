using System.CommandLine.Invocation;
using JetBrains.Annotations;

namespace Safir.CommandLine;

[PublicAPI]
public interface IHandlerBuilder : IHandlerBuilder<ConfiguredHandlerBuilder>
{
    ICommandHandler Build();
}
