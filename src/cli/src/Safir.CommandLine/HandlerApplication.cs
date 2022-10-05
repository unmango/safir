using System.CommandLine.Invocation;

namespace Safir.CommandLine;

internal sealed class HandlerApplication : ICommandHandler
{
    private readonly HandlerBuilder _builder;
    private readonly CommandHandler _handler;
    private HandlerContext? _context;

    public HandlerApplication(HandlerBuilder builder, CommandHandler handler)
    {
        _builder = builder;
        _handler = handler;
    }

    public HandlerContext GetContext(InvocationContext invocationContext)
    {
        _context ??= _builder.BuildHandlerContext(invocationContext);

        if (_context.InvocationContext != invocationContext)
            throw new InvalidOperationException("Something fucky is happening");

        return _context;
    }

    public int Invoke(InvocationContext context) => InvokeAsync(context).GetAwaiter().GetResult();

    public Task<int> InvokeAsync(InvocationContext context) => _handler(GetContext(context)).AsTask();
}
