using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using System.Threading.Tasks;
using Cli.Services;
using static Cli.Internal.ConsoleText;

namespace Cli.Commands.Service
{
    internal sealed class StatusCommand : Command
    {
        private static readonly ServiceArgument _services = new("The name of the service(s) to get the status of");
        
        public StatusCommand() : base("status", "Get the status of the selected service(s)")
        {
            AddArgument(_services);
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class StatusHandler : ICommandHandler
        {
            private readonly IServiceRegistry _registry;
            private readonly IServiceStatus _serviceStatus;

            public StatusHandler(IServiceRegistry registry, IServiceStatus serviceStatus)
            {
                _registry = registry;
                _serviceStatus = serviceStatus;
            }
            
            public Task<int> InvokeAsync(InvocationContext context)
            {
                Invoke(context);
                
                return Task.FromResult(0);
            }

            private void Invoke(InvocationContext context)
            {
                var selected = context.ParseResult.ValueForArgument(_services);
                
                // TODO: Probably log
                if (selected == null) return;

                var console = context.Console;
                var services = _registry.Find(selected);
                var statuses = _serviceStatus.GetStatus(services);

                foreach (var status in statuses)
                {
                    var values = GetStatusValues(status);
                    var table = CreateTable(values);
                    
                    console.Out.WriteLine(status.Name);
                    console.Append(table);
                }
            }

            private static TableView<StatusValues> CreateTable(IEnumerable<StatusValues> values)
            {
                var table = new TableView<StatusValues>();
                
                table.AddColumn(
                    x => x.Name,
                    Underline("Status"),
                    ColumnDefinition.SizeToContent());
                
                table.AddColumn(
                    x => x.Name,
                    Underline("Value"),
                    ColumnDefinition.Star(1));

                table.Items = values.ToList();

                return table;
            }

            private static IEnumerable<StatusValues> GetStatusValues(ServiceStatus status)
            {
                yield return new StatusValues(nameof(status.Installed), status.Installed);
                yield return new StatusValues(nameof(status.Health), status.Health);
                yield return new StatusValues(nameof(status.State), status.State);
            }

            private record StatusValues(string Name, object Value);
        }
    }
}
