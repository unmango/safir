using Safir.EndToEndTesting;
using Safir.Manager.Protos;
using Safir.Protos;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests;

public abstract class ManagerServiceTestBase : ServiceTestBase
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected ManagerServiceTestBase(ManagerServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    protected Host.HostClient GetHostClient() => new(CreateGrpcChannel());

    protected Media.MediaClient GetMediaClient() => new(CreateGrpcChannel());
}
