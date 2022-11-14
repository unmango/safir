using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Safir.Manager.Services;
using Safir.Protos;

namespace Safir.Manager.Tests.Services;

[Trait("Category", "Unit")]
public class AgentAggregatorTests
{
    private readonly Mock<IOptions<ManagerConfiguration>> _options = new();
    private readonly Mock<GrpcClientFactory> _clientFactory = new();
    private readonly FileSystem.FileSystemClient _fileSystemClient = Mock.Of<FileSystem.FileSystemClient>();
    private readonly Host.HostClient _hostClient = Mock.Of<Host.HostClient>();

    [Fact]
    public void FileSystem_CreatesSingleClient()
    {
        const string name = "test";
        _clientFactory.Setup(x => x.CreateClient<FileSystem.FileSystemClient>(name)).Returns(_fileSystemClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name] = new() { Uri = "https://example.com" },
            },
        });

        var result = Assert.Single(aggregator.FileSystem);
        Assert.Equal(name, result.Key);
        _clientFactory.Verify(x => x.CreateClient<FileSystem.FileSystemClient>(name));
        Assert.Same(_fileSystemClient, result.Value);
    }

    [Fact]
    public void FileSystem_CreatesMultipleClients()
    {
        const string name1 = "test1", name2 = "test2";
        _clientFactory.Setup(x => x.CreateClient<FileSystem.FileSystemClient>(name1)).Returns(_fileSystemClient);
        _clientFactory.Setup(x => x.CreateClient<FileSystem.FileSystemClient>(name2)).Returns(_fileSystemClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name1] = new() { Uri = "https://example.com" },
                [name2] = new() { Uri = "https://example.com" },
            },
        });

        Assert.Collection(aggregator.FileSystem,
            pair => {
                Assert.Equal(name1, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<FileSystem.FileSystemClient>(name1));
                Assert.Same(_fileSystemClient, pair.Value);
            },
            pair => {
                Assert.Equal(name2, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<FileSystem.FileSystemClient>(name2));
                Assert.Same(_fileSystemClient, pair.Value);
            });
    }

    [Fact]
    public void Host_CreatesSingleClient()
    {
        const string name = "test";
        _clientFactory.Setup(x => x.CreateClient<Host.HostClient>(name)).Returns(_hostClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name] = new() { Uri = "https://example.com" },
            },
        });

        var result = Assert.Single(aggregator.Host);
        Assert.Equal(name, result.Key);
        _clientFactory.Verify(x => x.CreateClient<Host.HostClient>(name));
        Assert.Same(_hostClient, result.Value);
    }

    [Fact]
    public void Host_CreatesMultipleClients()
    {
        const string name1 = "test1", name2 = "test2";
        _clientFactory.Setup(x => x.CreateClient<Host.HostClient>(name1)).Returns(_hostClient);
        _clientFactory.Setup(x => x.CreateClient<Host.HostClient>(name2)).Returns(_hostClient);
        var aggregator = Create(new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [name1] = new() { Uri = "https://example.com" },
                [name2] = new() { Uri = "https://example.com" },
            },
        });

        Assert.Collection(aggregator.Host,
            pair => {
                Assert.Equal(name1, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<Host.HostClient>(name1));
                Assert.Same(_hostClient, pair.Value);
            },
            pair => {
                Assert.Equal(name2, pair.Key);
                _clientFactory.Verify(x => x.CreateClient<Host.HostClient>(name2));
                Assert.Same(_hostClient, pair.Value);
            });
    }

    private AgentAggregator Create(ManagerConfiguration configuration)
    {
        _options.SetupGet(x => x.Value).Returns(configuration);
        return new(_options.Object, _clientFactory.Object);
    }
}
