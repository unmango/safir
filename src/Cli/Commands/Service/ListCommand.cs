using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using System.Threading.Tasks;
using Cli.Services;
using Microsoft.Extensions.Options;

namespace Cli.Commands.Service
{
    internal sealed class ListCommand : Command
    {
        public ListCommand() : base("list", "List services")
        {
            AddAlias("ls");
        }

        public sealed class ListHandler : ICommandHandler
        {
            private readonly IOptions<Cli.Service> _options;
            private readonly IConsole _console;

            public ListHandler(IOptions<Cli.Service> options, IConsole console)
            {
                _options = options;
                _console = console;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                var table = new TableView<ServiceEntry> {
                    Items = _options.Value.Values.ToList()
                };

                table.AddColumn(
                    entry => entry.Name,
                    Underline(nameof(ServiceEntry.Name)),
                    ColumnDefinition.SizeToContent());

                table.AddColumn(
                    entry => entry.Source,
                    Underline(nameof(ServiceEntry.Source)),
                    ColumnDefinition.SizeToContent());

                table.AddColumn(
                    entry => entry.Type,
                    Underline(nameof(ServiceEntry.Type)),
                    ColumnDefinition.SizeToContent());

                table.AddColumn(
                    entry => entry.GitCloneUrl,
                    Underline(nameof(ServiceEntry.GitCloneUrl)),
                    ColumnDefinition.Star(1));

                _console.Append(table, _console.DetectOutputMode());

                return Task.FromResult(context.ResultCode);
            }

            private static View Underline(string text)
            {
                return new ContentView(new ContainerSpan(
                    StyleSpan.UnderlinedOn(),
                    new ContentSpan(text),
                    StyleSpan.UnderlinedOff()));
            }
        }
    }
}
