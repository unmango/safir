using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace System.CommandLine
{
    public abstract class CommandHandlerBase<T> : ICommandHandler
    {
        public Task<int> InvokeAsync(InvocationContext context)
        {
            return CommandHandler.Create(Execute).InvokeAsync(context);
        }

        protected abstract Task Execute();
    }
}
