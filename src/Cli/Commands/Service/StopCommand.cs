using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Cli.Commands.Service
{
    internal sealed class StopCommand : Command
    {
        public StopCommand() : base("stop", "Stop the selected service(s)")
        {
            AddOption(new ServiceOption());
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class StopHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
