using Microsoft.Extensions.Options;

namespace Safir.Agent;

internal sealed class AgentConfiguration
{
    public string? DataDirectory { get; set; }
}

internal sealed record AgentOptions
{
    public required string DataDirectory { get; init; }
}

internal static class OptionsExtensions
{
    public static AgentOptions Parse(this IOptions<AgentConfiguration> options)
    {
        var v = options.Value;

        ArgumentException.ThrowIfNullOrEmpty(v.DataDirectory);

        return new AgentOptions {
            DataDirectory = v.DataDirectory,
        };
    }
}
