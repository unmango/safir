using System.CommandLine;
using System.CommandLine.IO;
using Microsoft.Extensions.Options;
using Moq;
using Safir.Cli.Commands.Config;
using Safir.Cli.Commands.Config.Add;
using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Commands.Config.Add;

[Trait("Category", "Unit")]
public class AgentCommandTests
{
    private readonly Mock<IStandardStreamWriter> _outWriter = new();
    private readonly Mock<IConsole> _console = new();
    private readonly Mock<IOptions<SafirOptions>> _options = new();
    private readonly Mock<IUserConfiguration> _configuration = new();
    private readonly AgentCommand.Handler _handler;

    private readonly SafirOptions _defaultOptions = new() {
        Agents = new List<AgentOptions>(),
    };

    public AgentCommandTests()
    {
        _console.SetupGet(x => x.Out).Returns(_outWriter.Object);
        _options.SetupGet(x => x.Value).Returns(_defaultOptions);

        _handler = new(_console.Object, _options.Object, _configuration.Object);
    }

    [Fact]
    public void Value_RequiresServiceArgument()
    {
        var command = AgentCommand.Value;

        Assert.Contains(AddCommand.ServiceArgument, command.Arguments);
    }

    [Fact]
    public void Value_RequiresUriArgument()
    {
        var command = AgentCommand.Value;

        Assert.Contains(AddCommand.UriArgument, command.Arguments);
    }

    [Fact]
    public void Value_ValidatesUriArgument()
    {
        var command = AgentCommand.Value;

        var result = command.Parse("test not-a-uri");

        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, x => x.Message.StartsWith("Invalid URI"));
    }

    [Fact]
    public void Value_SetsHandler()
    {
        var command = AgentCommand.Value;

        Assert.NotNull(command.Handler);
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("test")]
    public async Task Handler_ShortCircuitsWhenServiceAlreadyExists(string expectedName)
    {
        var parseResult = AgentCommand.Value.Parse($"{expectedName} https://example.com");
        _options.SetupGet(x => x.Value).Returns(_defaultOptions with {
            Agents = new[] {
                new AgentOptions {
                    Name = expectedName.ToLower(),
                },
            },
        }).Verifiable();

        await _handler.Execute(parseResult);

        _options.Verify();
        _outWriter.Verify(x => x.Write(It.IsRegex(".*already configured")));
        _configuration.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handler_AddsNewService()
    {
        const string expectedName = "test";
        const string expectedUri = "https://example.com";
        var parseResult = AgentCommand.Value.Parse($"{expectedName} {expectedUri}");
        Action<LocalConfiguration>? update = null;
        _configuration.Setup(x => x.UpdateAsync(
                It.IsAny<Action<LocalConfiguration>>(),
                It.IsAny<CancellationToken>()))
            .Callback((Action<LocalConfiguration> action, CancellationToken _) => update = action);

        await _handler.Execute(parseResult);

        var testConfig = new LocalConfiguration(new List<AgentConfiguration>(), new List<ManagerConfiguration>());
        update?.Invoke(testConfig);
        Assert.Contains(testConfig.Agents, x => x is {
            Name: expectedName, Uri.OriginalString: expectedUri,
        });
        _outWriter.Verify(x => x.Write(It.IsRegex(".*[Aa]dded.*")));
    }
}
