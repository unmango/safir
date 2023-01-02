using JetBrains.Annotations;
using Safir.EndToEndTesting;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests;

[UsedImplicitly]
public sealed class ManagerFixture : SafirFixture
{
    public ManagerFixture(IMessageSink sink) : base(sink, "manager") { }
}
