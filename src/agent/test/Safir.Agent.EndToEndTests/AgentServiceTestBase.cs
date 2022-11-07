using Safir.Agent.Protos;
using Safir.EndToEndTesting;
using Safir.Protos;
using Xunit.Abstractions;

namespace Safir.Agent.EndToEndTests;

public abstract class AgentServiceTestBase : ServiceTestBase
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected AgentServiceTestBase(AgentServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    protected Host.HostClient GetHostClient() => new(CreateGrpcChannel());

    protected FileSystem.FileSystemClient GetFileSystemClient() => new(CreateGrpcChannel());
}
