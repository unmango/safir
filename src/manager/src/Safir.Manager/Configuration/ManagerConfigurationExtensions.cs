namespace Safir.Manager.Configuration;

internal static class ManagerConfigurationExtensions
{
    public static IEnumerable<AgentOptions> GetAgentOptions(this ManagerConfiguration? configuration)
        => configuration?.Agents?
               .Where(x => !string.IsNullOrWhiteSpace(x.Value.Uri))
               .Select(x => new AgentOptions(x.Key, new(x.Value.Uri!)))
           ?? Enumerable.Empty<AgentOptions>();
}
