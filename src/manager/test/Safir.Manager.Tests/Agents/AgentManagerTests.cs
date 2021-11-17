using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Safir.Manager.Agents;
using Safir.Manager.Configuration;
using Xunit;

namespace Safir.Manager.Tests.Agents
{
    public class AgentManagerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IOptionsMonitor<ManagerOptions>> _optionsMonitor;
        private readonly Mock<AgentFactory> _agentFactory;
        private readonly Mock<IAgent> _agent = new();
        private Action<ManagerOptions, string>? _changeCallback;
        private readonly AgentManager _manager;

        public AgentManagerTests()
        {
            _optionsMonitor = _mocker.GetMock<IOptionsMonitor<ManagerOptions>>();
            _optionsMonitor.Setup(x => x.OnChange(It.IsAny<Action<ManagerOptions, string>>()))
                .Callback<Action<ManagerOptions, string>>(x => _changeCallback = x);

            _agentFactory = _mocker.GetMock<AgentFactory>();
            _agentFactory.Setup(x => x.Create(It.IsAny<string>()))
                .Returns(_agent.Object);

            _manager = _mocker.CreateInstance<AgentManager>();
        }

        [Fact]
        public void Throws_WhenArgsAreNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new AgentManager(_optionsMonitor.Object, null!, null!));
            
            Assert.Throws<ArgumentNullException>(
                () => new AgentManager(null!, _agentFactory.Object, null!));
            
            Assert.Throws<ArgumentNullException>(
                () => new AgentManager(null!, null!, new Mock<ILogger<AgentManager>>().Object));
        }

        [Theory]
        [MemberData(nameof(AgentOptionsTestData))]
        public void EnumeratesAgents(IEnumerable<AgentOptions> agentOptions, IEnumerable<string> urls)
        {
            _optionsMonitor.SetupGet(x => x.CurrentValue)
                .Returns(new ManagerOptions {
                    Agents = agentOptions.ToList()
                });

            var result = _manager.Count();

            Assert.Equal(urls.Count(), result);
        }

        [Theory]
        [MemberData(nameof(AgentOptionsTestData))]
        public void UsesNameAsIndex(IEnumerable<AgentOptions> agentOptions, IEnumerable<string> names)
        {
            _optionsMonitor.SetupGet(x => x.CurrentValue)
                .Returns(new ManagerOptions {
                    Agents = agentOptions.ToList()
                });

            foreach (var name in names)
            {
                Assert.NotNull(_manager[name]);
            }
        }

        [Theory]
        [MemberData(nameof(AgentOptionsTestData))]
        public void CreatesAgentFromFactory(IEnumerable<AgentOptions> agentOptions, IEnumerable<string> names)
        {
            _optionsMonitor.SetupGet(x => x.CurrentValue)
                .Returns(new ManagerOptions {
                    Agents = agentOptions.ToList()
                });

            _ = _manager.ToList();

            foreach (var name in names)
            {
                _agentFactory.Verify(x => x.Create(name));
            }
        }

        public static IEnumerable<object[]> AgentOptionsTestData()
        {
            yield return new object[] {
                new AgentOptions[] {
                    new() { Name = "test" }
                },
                new[] { "test" }
            };

            yield return new object[] {
                new AgentOptions[] {
                    new() { Name = "test" },
                    new() { Name = "test2" }
                },
                new[] {
                    "test",
                    "test2"
                }
            };
        }

        [Fact]
        public void CreatesNewAgentWhenOptionsChange()
        {
            _optionsMonitor.SetupGet(x => x.CurrentValue)
                .Returns(new ManagerOptions { Agents = new() });

            _ = _manager.ToList();

            Assert.NotNull(_changeCallback);

            _changeCallback!(new() {
                Agents = new() {
                    new() {
                        Name = "https://TestUrl69"
                    }
                }
            }, "doesn't matter");

            Assert.NotNull(_manager["https://TestUrl69"]);
        }
    }
}
