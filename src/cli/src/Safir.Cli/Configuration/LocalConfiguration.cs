using System.Collections.Immutable;

namespace Safir.Cli.Configuration;

internal record LocalConfiguration(IImmutableList<AgentOptions> Agents);


internal record AgentOptions(string Name, string Uri);
