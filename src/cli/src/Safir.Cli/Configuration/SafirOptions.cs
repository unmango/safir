using System.Collections.Generic;
using System.IO;

namespace Safir.Cli.Configuration;

internal record SafirOptions
{
    public ICollection<AgentOptions> Agents { get; } = new List<AgentOptions>();

    public ConfigOptions Config { get; init; } = new();
}

internal record AgentOptions
{
    public string Name { get; set; } = string.Empty;

    public string Uri { get; set; } = string.Empty;
}

internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public string File => Path.Join(Directory, SafirDefaults.ConfigFile);
}
