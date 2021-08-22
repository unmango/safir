using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Moq;
using Moq.AutoMock;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Manager.Agents;
using Xunit;

namespace Safir.Manager.Tests.Agents
{
    public class AgentAggregatorTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IAgent> _agent;
        private readonly Mock<IFileSystemClient> _fileSystem;
        private readonly AgentAggregator _aggregator;

        public AgentAggregatorTests()
        {
            _agent = _mocker.GetMock<IAgent>();
            _agent.SetupGet(x => x.Name).Returns("test-name");
            _mocker.Use(typeof(IEnumerable<IAgent>), new[] { _agent.Object });
            _fileSystem = _mocker.GetMock<IFileSystemClient>();
            _agent.SetupGet(x => x.FileSystem).Returns(_fileSystem.Object);
            _aggregator = _mocker.CreateInstance<AgentAggregator>();
        }

        [Fact]
        public async Task List_SetsHost()
        {
            const string expected = "expected";
            _agent.SetupGet(x => x.Name).Returns(expected);
            _fileSystem.Setup(x => x.ListFilesAsync(It.IsAny<CancellationToken>()))
                .Returns(AsyncEnumerable.Repeat(new FileSystemEntry(), 1));

            var result = await _aggregator.List(default).ToListAsync();

            var item = Assert.Single(result);
            Assert.Equal(expected, item.Host);
        }

        [Fact]
        public async Task List_SetsPath()
        {
            const string expected = "expected";
            _fileSystem.Setup(x => x.ListFilesAsync(It.IsAny<CancellationToken>()))
                .Returns(AsyncEnumerable.Repeat(new FileSystemEntry {
                    Path = expected,
                }, 1));

            var result = await _aggregator.List(default).ToListAsync();

            var item = Assert.Single(result);
            Assert.Equal(expected, item.Path);
        }

        [Fact]
        public async Task List_AggregatesHosts()
        {
            // TODO: Consider refactor because of this garbage
            var fs2 = new Mock<IFileSystemClient>();
            var agent2 = new Mock<IAgent>();
            agent2.SetupGet(x => x.FileSystem).Returns(fs2.Object);
            _mocker.Use(typeof(IEnumerable<IAgent>), new[] { _agent.Object, agent2.Object });
            var aggregator = _mocker.CreateInstance<AgentAggregator>();
            
            _agent.SetupGet(x => x.Name).Returns("host1");
            agent2.SetupGet(x => x.Name).Returns("host2");
            _fileSystem.Setup(x => x.ListFilesAsync(It.IsAny<CancellationToken>()))
                .Returns(AsyncEnumerable.Repeat(new FileSystemEntry {
                    Path = "path1"
                }, 1));
            fs2.Setup(x => x.ListFilesAsync(It.IsAny<CancellationToken>()))
                .Returns(AsyncEnumerable.Repeat(new FileSystemEntry {
                    Path = "path2"
                }, 1));

            var result = await aggregator.List(default).ToListAsync();
            
            Assert.Collection(result,
                [AssertionMethod]
                (x) => {
                    Assert.Equal("host1", x.Host);
                    Assert.Equal("path1", x.Path);
                },
                [AssertionMethod]
                (x) => {
                    Assert.Equal("host2", x.Host);
                    Assert.Equal("path2", x.Path);
                });
        }
    }
}
