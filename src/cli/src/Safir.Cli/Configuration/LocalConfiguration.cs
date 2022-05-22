using System;
using System.Collections.Generic;

namespace Safir.Cli.Configuration;

internal record LocalConfiguration(IList<AgentConfiguration> Agents)
{
    public LocalConfiguration() : this(new List<AgentConfiguration>()) { }
}

internal record AgentConfiguration(string Name, Uri Uri);
