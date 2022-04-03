using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Safir.Cli.Commands.Service;

namespace Safir.Cli.Commands;

internal sealed class ServiceCommand : Command
{
    public ServiceCommand() : base("service", "Control various Safir services")
    {
        AddAlias("s");
        AddCommand(new EnableCommand());
        AddCommand(new InstallCommand());
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
        .UseCommandHandler<InstallCommand, InstallCommand.InstallHandler>()
        .UseCommandHandler<ListCommand, ListCommand.ListHandler>()
        .UseCommandHandler<RestartCommand, RestartCommand.RestartHandler>()
        .UseCommandHandler<StartCommand, StartCommand.StartHandler>()
        .UseCommandHandler<StatusCommand, StatusCommand.StatusHandler>()
        .UseCommandHandler<StopCommand, StopCommand.StopHandler>();
}