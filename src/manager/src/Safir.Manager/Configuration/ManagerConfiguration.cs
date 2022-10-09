namespace Safir.Manager.Configuration;

internal sealed class ManagerConfiguration
{
    public IDictionary<string, AgentConfiguration>? Agents { get; init; }
}
