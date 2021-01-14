using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using System.Threading.Tasks;
using Cli.Services;
using static Cli.Internal.ConsoleText;

namespace Cli.Commands.Service
{
    internal sealed class ListCommand : Command
    {
        public ListCommand() : base("list", "List services")
        {
            AddAlias("ls");
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class ListHandler : ICommandHandler
        {
            private readonly IServiceRegistry _services;
            private readonly IConsole _console;

            public ListHandler(IServiceRegistry services, IConsole console)
            {
                _services = services;
                _console = console;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                var table = new TableView<KeyValuePair<string, IService>> {
                    Items = _services.ToList()
                };

                table.AddColumn(
                    entry => entry.Key,
                    Underline("Name"),
                    ColumnDefinition.SizeToContent());

                table.AddColumn(
                    entry => entry.Value.Sources.Count(),
                    Underline("Configured Sources"),
                    ColumnDefinition.Star(1));

                // table.AddColumn(
                //     entry => entry.SourceType,
                //     Underline(nameof(ServiceEntry.SourceType)),
                //     ColumnDefinition.SizeToContent());
                //
                // table.AddColumn(
                //     entry => entry.Type,
                //     Underline(nameof(ServiceEntry.Type)),
                //     ColumnDefinition.SizeToContent());
                //
                // table.AddColumn(
                //     entry => entry.GitCloneUrl,
                //     Underline(nameof(ServiceEntry.GitCloneUrl)),
                //     ColumnDefinition.Star(1));

                _console.Append(table, _console.DetectOutputMode());

                return Task.FromResult(context.ResultCode);
            }
        }
    }
}
