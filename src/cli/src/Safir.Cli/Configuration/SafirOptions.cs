using System.Collections.Generic;
using System.IO;

namespace Safir.Cli.Configuration;

internal record SafirOptions
{
    public ConfigOptions Config { get; init; } = new();
}

internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public string File => Path.Join(Directory, SafirDefaults.ConfigFile);
}
