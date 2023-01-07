using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.V1Alpha1;
using Safir.Manager.Services;

namespace Safir.Manager.Tests.Services;

[Trait("Category", "Unit")]
public class AgentAggregatorTests
{
    private readonly Mock<IOptions<ManagerConfiguration>> _options = new();
    private readonly Mock<GrpcClientFactory> _clientFactory = new();
    private readonly FilesService.FilesServiceClient _fileSystemClient = Mock.Of<FilesService.FilesServiceClient>();
    private readonly Common.V1Alpha1.HostService.HostServiceClient _hostClient = Mock.Of<Common.V1Alpha1.HostService.HostServiceClient>();

    [Fact]
    public void FileSystem_CreatesSingleClient()
    {
        const string name = "test", clientName = $"{name}-filesystem";
        _clientFactory.Setup(x => x.CreateClient<FilesService.FilesServiceClient>(clientName)).Returns(_fileSystemClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name] = new() { Uri = "https://example.com" },
            },
        });

        var result = Assert.Single(aggregator.FileSystem);
        Assert.Equal(name, result.Key);
        _clientFactory.Verify(x => x.CreateClient<FilesService.FilesServiceClient>(clientName));
        Assert.Same(_fileSystemClient, result.Value);
    }

    [Fact]
    public void FileSystem_CreatesMultipleClients()
    {
        const string name1 = "test1", name2 = "test2";
        const string clientName1 = $"{name1}-filesystem", clientName2 = $"{name2}-filesystem";

        _clientFactory.Setup(x => x.CreateClient<FilesService.FilesServiceClient>(clientName1)).Returns(_fileSystemClient);
        _clientFactory.Setup(x => x.CreateClient<FilesService.FilesServiceClient>(clientName2)).Returns(_fileSystemClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name1] = new() { Uri = "https://example.com" },
                [name2] = new() { Uri = "https://example.com" },
            },
        });

        Assert.Collection(aggregator.FileSystem,
            pair => {
                Assert.Equal(name1, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<FilesService.FilesServiceClient>(clientName1));
                Assert.Same(_fileSystemClient, pair.Value);
            },
            pair => {
                Assert.Equal(name2, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<FilesService.FilesServiceClient>(clientName2));
                Assert.Same(_fileSystemClient, pair.Value);
            });
    }

    [Fact]
    public void Host_CreatesSingleClient()
    {
        const string name = "test", clientName = $"{name}-host";
        _clientFactory.Setup(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName)).Returns(_hostClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name] = new() { Uri = "https://example.com" },
            },
        });

        var result = Assert.Single(aggregator.Host);
        Assert.Equal(name, result.Key);
        _clientFactory.Verify(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName));
        Assert.Same(_hostClient, result.Value);
    }

    [Fact]
    public void Host_CreatesMultipleClients()
    {
        const string name1 = "test1", name2 = "test2";
        const string clientName1 = $"{name1}-host", clientName2 = $"{name2}-host";

        _clientFactory.Setup(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName1)).Returns(_hostClient);
        _clientFactory.Setup(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName2)).Returns(_hostClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name1] = new() { Uri = "https://example.com" },
                [name2] = new() { Uri = "https://example.com" },
            },
        });

        Assert.Collection(aggregator.Host,
            pair => {
                Assert.Equal(name1, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName1));
                Assert.Same(_hostClient, pair.Value);
            },
            pair => {
                Assert.Equal(name2, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<Common.V1Alpha1.HostService.HostServiceClient>(clientName2));
                Assert.Same(_hostClient, pair.Value);
            });
    }

    private AgentAggregator Create(ManagerConfiguration configuration)
    {
        _options.SetupGet(x => x.Value).Returns(configuration);
        return new(_options.Object, _clientFactory.Object);
    }
}
