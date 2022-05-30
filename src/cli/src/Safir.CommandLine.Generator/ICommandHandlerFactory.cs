using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.CommandLine;

internal interface IHandlerBuilderCreator<T>
{
    ICommandHandler Create();
}

internal class TestHandler
{
    public Task<int> Execute(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }
}

internal class TestImpl : IHandlerBuilderCreator<TestHandler>
{

    ICommandHandler IHandlerBuilderCreator<TestHandler>.Create()
    {
        throw new System.NotImplementedException();
    }

    private class TestConcreteHandler : ICommandHandler
    {

        public Task<int> InvokeAsync(InvocationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
