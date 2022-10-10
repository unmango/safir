namespace Safir.Manager;

internal sealed class AgentConfiguration
{
    public string? Uri { get; init; }
}

internal sealed class ManagerConfiguration
{
    public IDictionary<string, AgentConfiguration>? Agents { get; init; }
}

internal sealed record AgentOptions(string Name, Uri Uri);

internal static class ManagerConfigurationExtensions
{
    public static IEnumerable<AgentOptions> GetAgentOptions(this ManagerConfiguration? configuration)
        => configuration?.Agents?
               .Where(x => !string.IsNullOrWhiteSpace(x.Value.Uri))
               .Select(x => new AgentOptions(x.Key, new(x.Value.Uri!)))
           ?? Enumerable.Empty<AgentOptions>();
}
