using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;

namespace Safir.CommandLine.Tests;

[Trait("Category", "Unit")]
public class HandlerContextExtensionsTests
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly ParseResult _parseResult = new RootCommand().Parse(string.Empty);
    private readonly InvocationContext _invocationContext;
    private readonly Mock<IServiceProvider> _services = new();
    private readonly HandlerContext _context;

    public HandlerContextExtensionsTests()
    {
        _invocationContext = new(_parseResult);
        _context = new(_configuration.Object, _invocationContext, _services.Object);
    }

    [Fact]
    public void GetCancellationToken_GetsInvocationContextToken()
    {
        var expected = _invocationContext.GetCancellationToken();

        var result = _context.GetCancellationToken();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetParseResult_GetsInvocationContextParseResult()
    {
        var expected = _invocationContext.ParseResult;

        var result = _context.GetParseResult();

        Assert.Same(expected, result);
    }

    // TODO: GetService* tests
}
