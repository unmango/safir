using JetBrains.Annotations;
using Safir.EndToEndTesting;

namespace Safir.Manager.EndToEndTests;

[UsedImplicitly]
public sealed class ManagerFixture : SafirFixture
{
    public ManagerFixture() : base("manager") { }
}
