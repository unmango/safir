using System.CommandLine;
using System.CommandLine.Hosting;
using Cli.Commands.Service;
using Microsoft.Extensions.Hosting;

namespace Cli.Commands
{
    internal sealed class ServiceCommand : Command
    {
        public ServiceCommand() : base("service", "Control various Safir services")
        {
            AddAlias("s");
            AddCommand(new EnableCommand());
            AddCommand(new ListCommand());
            AddCommand(new RestartCommand());
            AddCommand(new StartCommand());
            AddCommand(new StatusCommand());
            AddCommand(new StopCommand());
        }
    }

    internal static class ServiceCommandExtensions
    {
        public static IHostBuilder AddServiceCommand(this IHostBuilder builder) => builder
            .UseCommandHandler<EnableCommand, EnableCommand.EnableHandler>()
            .UseCommandHandler<ListCommand, ListCommand.ListHandler>()
            .UseCommandHandler<RestartCommand, RestartCommand.RestartHandler>()
            .UseCommandHandler<StartCommand, StartCommand.StartHandler>()
            .UseCommandHandler<StatusCommand, StatusCommand.StatusHandler>()
            .UseCommandHandler<StopCommand, StopCommand.StopHandler>();
    }
}
