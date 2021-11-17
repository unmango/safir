using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Cli.Commands.Service
{
    internal sealed class RestartCommand : Command
    {
        public RestartCommand() : base("restart", "Restart the selected service(s)")
        {
            AddAlias("r");
            AddOption(new ServiceOption());
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class RestartHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
