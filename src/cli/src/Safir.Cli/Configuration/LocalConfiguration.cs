namespace Safir.Cli.Configuration;

internal record LocalConfiguration(
    IList<AgentConfiguration> Agents,
    IList<ManagerConfiguration> Managers)
{
    public LocalConfiguration()
        : this(
            new List<AgentConfiguration>(),
            new List<ManagerConfiguration>()) { }

    public static LocalConfiguration Empty() => new();
}

internal abstract record ServiceConfiguration(string Name, Uri Uri);

internal sealed record AgentConfiguration(string Name, Uri Uri)
    : ServiceConfiguration(Name, Uri);

internal sealed record ManagerConfiguration(string Name, Uri Uri)
    : ServiceConfiguration(Name, Uri);
