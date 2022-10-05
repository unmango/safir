using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine.Tests;

public class HandlerBuilderExtensionsTests
{
    private readonly InvocationContext _invocationContext;
    private readonly HandlerBuilderContext _builderContext;
    private readonly Mock<IConfigurationBuilder> _configurationBuilder = new();
    private readonly Mock<IServiceCollection> _services = new();
    private readonly Mock<IHandlerBuilder> _builder = new();

    public HandlerBuilderExtensionsTests()
    {
        var parseResult = new RootCommand().Parse(string.Empty);
        _invocationContext = new(parseResult);
        _builderContext = new(Mock.Of<IConfiguration>(), _invocationContext);
    }

    [Fact]
    public void ConfigureAppConfiguration_ForwardsDelegate()
    {
        var source = Mock.Of<IConfigurationSource>();
        Action<HandlerBuilderContext, IConfigurationBuilder>? callback = null;
        _builder
            .Setup(x => x.ConfigureAppConfiguration(It.IsAny<Action<HandlerBuilderContext, IConfigurationBuilder>>()))
            .Callback((Action<HandlerBuilderContext, IConfigurationBuilder> c) => callback = c);

        _builder.Object.ConfigureAppConfiguration(b => b.Add(source));

        Assert.NotNull(callback);
        callback!.Invoke(_builderContext, _configurationBuilder.Object);
        _configurationBuilder.Verify(x => x.Add(source));
    }

    // TODO: ConfigureHandler tests

    [Fact]
    public void ConfigureHostConfiguration_ForwardsDelegate()
    {
        var source = Mock.Of<IConfigurationSource>();
        Action<InvocationContext, IConfigurationBuilder>? callback = null;
        _builder
            .Setup(x => x.ConfigureHostConfiguration(It.IsAny<Action<InvocationContext, IConfigurationBuilder>>()))
            .Callback((Action<InvocationContext, IConfigurationBuilder> c) => callback = c);

        _builder.Object.ConfigureHostConfiguration(b => b.Add(source));

        Assert.NotNull(callback);
        callback!.Invoke(_invocationContext, _configurationBuilder.Object);
        _configurationBuilder.Verify(x => x.Add(source));
    }

    [Fact]
    public void ConfigureServices_ForwardsDelegate()
    {
        var descriptor = ServiceDescriptor.Singleton(new object());
        Action<HandlerBuilderContext, IServiceCollection>? callback = null;
        _builder
            .Setup(x => x.ConfigureServices(It.IsAny<Action<HandlerBuilderContext, IServiceCollection>>()))
            .Callback((Action<HandlerBuilderContext, IServiceCollection> c) => callback = c);

        _builder.Object.ConfigureServices(b => b.Add(descriptor));

        Assert.NotNull(callback);
        callback!.Invoke(_builderContext, _services.Object);
        _services.Verify(x => x.Add(descriptor));
    }

    [Fact]
    public void SetHandler_BuildsHandler()
    {
        Command command = new("test");

        _builder.Object.SetHandler(command);

        _builder.Verify(x => x.Build());
    }
}
