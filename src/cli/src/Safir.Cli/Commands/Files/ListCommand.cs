using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Safir.CommandLine.Generator;

namespace Safir.Cli.Commands.Files;

internal static class ListCommand
{

    internal class ListCommandHandler
    {
        [CommandHandler]
        public async Task HandleAsync(ParseResult parseResult, CancellationToken cancellationToken)
        {
        }
    }
}
