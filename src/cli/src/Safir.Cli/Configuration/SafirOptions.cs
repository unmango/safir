using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Cli.Configuration;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal record SafirOptions
{
    // TODO: Binder pukes when value is an empty array
    public IEnumerable<AgentOptions> Agents { get; init; } = null!; // Initialized via binder

    public ConfigOptions Config { get; init; } = new();
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal record AgentOptions
{
    public string Name { get; init; } = string.Empty;

    public string Uri { get; init; } = string.Empty;
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public string File { get; init; } = string.Empty;
}
