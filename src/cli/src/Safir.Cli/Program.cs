using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Safir.Cli.Commands;
using Safir.CommandLine;

var command = new RootCommand("CLI for interacting with Safir services");

command.AddCommand(ConfigCommand.Value);

return await new CommandLineBuilder(command)
    .UseDefaults()
    .UseCommandHandlers(ConfigCommand.CommandHandlers)
    .Build()
    .InvokeAsync(args);
