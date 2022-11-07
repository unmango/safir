using JetBrains.Annotations;
using Safir.EndToEndTesting;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests;

[UsedImplicitly]
public sealed class ManagerServiceFixture : ServiceFixtureBase
{
    public ManagerServiceFixture(IMessageSink sink)
        : base(sink, "safir-manager:e2e", "manager/Dockerfile") { }
}
