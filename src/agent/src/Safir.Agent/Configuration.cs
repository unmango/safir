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

internal sealed record FileWatcherOptions
{
    public required string Path { get; set; }
}

internal static class OptionsExtensions
{
    public static AgentOptions Parse(this AgentConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(configuration.DataDirectory);

        return new AgentOptions {
            DataDirectory = configuration.DataDirectory,
        };
    }

    public static AgentOptions Parse(this IOptions<AgentConfiguration> options)
        => Parse(options.Value);
}
