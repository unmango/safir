namespace Safir.Agent.EndToEndTests;

[CollectionDefinition(Name)]
public class AgentServiceCollection : ICollectionFixture<AgentServiceFixture>
{
    public const string Name = "Agent Service";
}
