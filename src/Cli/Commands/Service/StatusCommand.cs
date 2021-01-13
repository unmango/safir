using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;

namespace Cli.Commands.Service
{
    internal sealed class StatusCommand : Command
    {
        public StatusCommand() : base("status", "Get the status of the selected service(s)")
        {
            AddArgument(new ServiceArgument());
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class StatusHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                context.Console.Out.WriteLine($"Installed: {true}");

                return Task.FromResult(0);
            }
        }
    }
}
