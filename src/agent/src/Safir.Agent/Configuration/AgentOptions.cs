using JetBrains.Annotations;

namespace Safir.Agent.Configuration;

internal class AgentOptions
{
    public static readonly ImmutableDictionary<string, string> CliSwitches = new Dictionary<string, string> {
        ["--pipe-handle"] = nameof(PipeHandle),
    }.ToImmutableDictionary();

    public string? DataDirectory { get; set; }

    public bool EnableGrpcReflection { get; [UsedImplicitly] set; }

    public bool EnableSwagger { get; [UsedImplicitly] set; }

    public EnumerationOptions? EnumerationOptions { get; [UsedImplicitly] set; }

    public int MaxDepth { get; [UsedImplicitly] set; }

    public string? PipeHandle { get; set; }

    public string Redis { get; set; } = string.Empty;

    public bool UsePipeLifetime => !string.IsNullOrWhiteSpace(PipeHandle);
}
