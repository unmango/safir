using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Cli.Commands.Service
{
    internal sealed class StartCommand : Command
    {
        public StartCommand() : base("start", "Start the selected service(s)")
        {
            AddOption(ServiceOption.Value);
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class StartHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
