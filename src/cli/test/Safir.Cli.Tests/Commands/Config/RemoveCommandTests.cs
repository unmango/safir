using System.CommandLine;
using System.CommandLine.IO;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Cli.Commands.Config;
using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Commands.Config;

public class RemoveCommandTests
{
    private readonly Mock<IStandardStreamWriter> _outWriter = new();
    private readonly Mock<IConsole> _console = new();
    private readonly Mock<IOptionsMonitor<SafirOptions>> _options = new();
    private readonly Mock<IUserConfiguration> _configuration = new();
    private readonly RemoveCommand.RemoveCommandHandler _handler;

    private readonly SafirOptions _defaultOptions = new() {
        Agents = new List<AgentOptions>(),
    };

    public RemoveCommandTests()
    {
        _console.SetupGet(x => x.Out).Returns(_outWriter.Object);
        _options.SetupGet(x => x.CurrentValue).Returns(_defaultOptions);

        _handler = new(_console.Object, _options.Object, _configuration.Object);
    }

    [Fact]
    public void Value_HasShorthandAlias()
    {
        var command = RemoveCommand.Value;

        Assert.Contains("rm", command.Aliases);
    }

    [Fact]
    public void Value_RequiresServiceArgument()
    {
        var command = RemoveCommand.Value;

        Assert.Contains(RemoveCommand.ServiceArgument, command.Arguments);
    }

    [Fact]
    public async Task Handler_ShortCircuitsWhenNoServiceConfigured()
    {
        var parseResult = RemoveCommand.Value.Parse("test");

        await _handler.Execute(parseResult);

        _configuration.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handler_ShortCircuitsWhenDifferentServiceConfigured()
    {
        var parseResult = RemoveCommand.Value.Parse("test");
        _options.SetupGet(x => x.CurrentValue).Returns(_defaultOptions with {
            Agents = new[] {
                new AgentOptions {
                    Name = "other",
                },
            },
        }).Verifiable();

        await _handler.Execute(parseResult);

        _options.Verify();
        _configuration.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("test")]
    [InlineData("Test")]
    public async Task Handler_RemovesService(string service)
    {
        var parseResult = RemoveCommand.Value.Parse(service);
        _options.SetupGet(x => x.CurrentValue).Returns(_defaultOptions with {
            Agents = new[] {
                new AgentOptions {
                    Name = service.ToLower(),
                },
            },
        }).Verifiable();
        Action<LocalConfiguration>? update = null;
        _configuration.Setup(x => x.UpdateAsync(
                It.IsAny<Action<LocalConfiguration>>(),
                It.IsAny<CancellationToken>()))
            .Callback((Action<LocalConfiguration> action, CancellationToken _) => update = action);

        await _handler.Execute(parseResult);

        _options.Verify();
        var testConfig = new LocalConfiguration(new List<AgentConfiguration> {
            new(service, new Uri("https://example.com")),
        }, new List<ManagerConfiguration>());
        update?.Invoke(testConfig);
        Assert.Empty(testConfig.Agents);
    }
}
