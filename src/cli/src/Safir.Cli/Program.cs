using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

const string description = "CLI for interacting with Safir services";

var command = new RootCommand(description);

return await new CommandLineBuilder(command)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);
