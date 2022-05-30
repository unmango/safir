using System.CommandLine.Invocation;

namespace Safir.CommandLine;

internal static class ConcreteHandler<THandler, TImp>
    where TImp : IHandlerBuilderCreator<THandler>
{
    public static ICommandHandler Instance { get; } = TImp.Create();
}
