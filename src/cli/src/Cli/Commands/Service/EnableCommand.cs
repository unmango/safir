using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Cli.Commands.Service
{
    public sealed class EnableCommand : Command
    {
        public EnableCommand() : base("enable", "Enable a service")
        {
            AddAlias("e");
        }

        public sealed class EnableHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
