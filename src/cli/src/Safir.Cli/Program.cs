﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Safir.Cli.Commands;

const string description = "CLI for interacting with Safir services";

var command = new RootCommand(description) {
    ConfigCommand.Value,
    MediaCommand.Value,
};

return await new CommandLineBuilder(command)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);
