using JetBrains.Annotations;
using Safir.EndToEndTesting;
using Xunit.Abstractions;

namespace Safir.Agent.EndToEndTests;

[UsedImplicitly]
public class AgentServiceFixture : ServiceFixtureBase
{
    public AgentServiceFixture(IMessageSink sink)
        : base(sink, "safir-agent:e2e", "agent/Dockerfile") { }
}
