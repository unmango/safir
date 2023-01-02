using JetBrains.Annotations;
using Safir.EndToEndTesting;

namespace Safir.Agent.EndToEndTests;

[UsedImplicitly]
public class AgentFixture : SafirFixture
{
    public AgentFixture() : base("agent") { }
}
