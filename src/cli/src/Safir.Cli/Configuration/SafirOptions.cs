using JetBrains.Annotations;

namespace Safir.Cli.Configuration;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal record SafirOptions
{
    // TODO: Binder hates enumerables
    public IEnumerable<AgentOptions>? Agents { get; init; }

    public ConfigOptions Config { get; init; } = new();

    public IEnumerable<ManagerOptions>? Managers { get; init; }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal abstract record ServiceOptions
{
    public string Name { get; init; } = string.Empty;

    public string Uri { get; init; } = string.Empty;
}

internal sealed record AgentOptions : ServiceOptions;

internal sealed record ManagerOptions : ServiceOptions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public string File { get; init; } = string.Empty;
}
