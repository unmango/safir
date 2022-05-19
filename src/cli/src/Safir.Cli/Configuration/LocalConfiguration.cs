using System.Collections.Generic;

namespace Safir.Cli.Configuration;

internal record LocalConfiguration(IList<AgentOptions> Agents);

internal record AgentOptions(string Name, string Uri);
