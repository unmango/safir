using System.Collections.Generic;

namespace Safir.Cli.Configuration;

internal record LocalConfiguration(IList<AgentOptions> Agents);


internal record AgentOptions
{
    public string Name { get; set; } = string.Empty;

    public string Uri { get; set; } = string.Empty;
}
