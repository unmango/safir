using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine.Tests;

[Trait("Category", "Unit")]
public class HandlerBuilderTests
{
    private readonly Mock<IConsole> _console = new();
    private readonly InvocationContext _invocationContext;
    private readonly HandlerBuilder _builder = new();

    public HandlerBuilderTests()
    {
        var command = new RootCommand();
        var parseResult = command.Parse(string.Empty);
        _invocationContext = new(parseResult, _console.Object);
    }

    [Fact]
    public void Build_ThrowsWhenNoHandlerConfigured()
    {
        Assert.Throws<InvalidOperationException>(() => _builder.Build());
    }

    [Fact]
    public void Build_DoesntInvokeAnyDelegates()
    {
        var configuredServices = false;
        var configuredAppConfig = false;
        var configuredHostConfig = false;

        _ = _builder.ConfigureHandler(_ => new())
            .ConfigureServices((_, _) => configuredServices = true)
            .ConfigureAppConfiguration((_, _) => configuredAppConfig = true)
            .ConfigureHostConfiguration(_ => configuredHostConfig = true)
            .Build();

        Assert.False(configuredServices);
        Assert.False(configuredAppConfig);
        Assert.False(configuredHostConfig);
    }

    [Fact]
    public async Task Build_UsesInvocationContext()
    {
        InvocationContext? invoked = null;
        var handler = _builder.ConfigureHandler(context => {
            invoked = context.InvocationContext;
            return new();
        }).Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.Same(_invocationContext, invoked);
    }

    [Theory]
    [InlineData(typeof(HandlerBuilderContext))]
    [InlineData(typeof(IConfiguration))]
    [InlineData(typeof(InvocationContext))]
    [InlineData(typeof(BindingContext))]
    [InlineData(typeof(IConsole))]
    [InlineData(typeof(ParseResult))]
    public async Task Build_AddsCoreServicesToServiceProvider(Type serviceType)
    {
        object? invoked = null;
        var handler = _builder.ConfigureHandler(context => {
            invoked = context.Services.GetService(serviceType);
            return new();
        }).Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(invoked);
    }

    [Fact]
    public async Task Build_AddsSameConsoleAsInvocation()
    {
        object? invoked = null;
        var handler = _builder.ConfigureHandler(context => {
            invoked = context.Services.GetService(typeof(IConsole));
            return new();
        }).Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.Same(_console.Object, invoked);
    }

    [Fact]
    public async Task Build_UsesHostConfigurationInConfigureAppConfiguration()
    {
        KeyValuePair<string, string?> pair = new("key", "value");
        IConfiguration? invoked = null;
        var handler = _builder
            .ConfigureHostConfiguration((_, builder) => builder.AddInMemoryCollection(new[] { pair }))
            .ConfigureAppConfiguration((context, _) => invoked = context.Configuration)
            .ConfigureHandler(_ => new())
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(invoked);
        Assert.Equal(pair.Value, invoked![pair.Key]);
    }

    [Fact]
    public async Task Build_UsesHostConfigurationInConfigureServices()
    {
        KeyValuePair<string, string?> pair = new("key", "value");
        IConfiguration? invoked = null;
        var handler = _builder
            .ConfigureHostConfiguration((_, builder) => builder.AddInMemoryCollection(new[] { pair }))
            .ConfigureServices((context, _) => invoked = context.Configuration)
            .ConfigureHandler(_ => new())
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(invoked);
        Assert.Equal(pair.Value, invoked![pair.Key]);
    }

    [Fact]
    public async Task Build_UsesAppConfigurationInConfigureServices()
    {
        KeyValuePair<string, string?> pair = new("key", "value");
        IConfiguration? invoked = null;
        var handler = _builder
            .ConfigureAppConfiguration((_, builder) => builder.AddInMemoryCollection(new[] { pair }))
            .ConfigureServices((context, _) => invoked = context.Configuration)
            .ConfigureHandler(_ => new())
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(invoked);
        Assert.Equal(pair.Value, invoked![pair.Key]);
    }

    [Fact]
    public async Task ConfigureAppConfiguration_AddsConfiguration()
    {
        const string key = "key", value = "value";
        IConfiguration? configuration = null;
        var handler = _builder
            .ConfigureAppConfiguration((_, builder) => builder.AddInMemoryCollection(new[] {
                new KeyValuePair<string, string?>(key, value),
            }))
            .ConfigureHandler(context => {
                configuration = context.Configuration;
                return new();
            })
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(configuration);
        Assert.Equal(value, configuration![key]);
    }

    [Fact]
    public void ConfigureAppConfiguration_ReturnsBuilder()
    {
        var builder = _builder.ConfigureAppConfiguration((_, _) => { });

        Assert.Same(_builder, builder);
    }

    [Fact]
    public async Task ConfigureHandler_SetsHandler()
    {
        var flag = false;
        var handler = _builder.ConfigureHandler(_ => {
            flag = true;
            return new();
        }).Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.True(flag);
    }

    [Fact]
    public async Task ConfigureHandler_OverwritesPreviousHandler()
    {
        var first = false;
        var second = false;
        var handler = _builder
            .ConfigureHandler(_ => {
                first = true;
                return new();
            })
            .ConfigureHandler(_ => {
                second = true;
                return new();
            })
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.False(first);
        Assert.True(second);
    }

    [Fact]
    public void ConfigureHandler_ReturnsBuilder()
    {
        var builder = _builder.ConfigureHandler(_ => new());

        Assert.Same(_builder, builder);
    }

    [Fact]
    public async Task ConfigureHostConfiguration_AddsConfiguration()
    {
        const string key = "key", value = "value";
        IConfiguration? configuration = null;
        var handler = _builder
            .ConfigureHostConfiguration((_, builder) => builder.AddInMemoryCollection(new[] {
                new KeyValuePair<string, string?>(key, value),
            }))
            .ConfigureHandler(context => {
                configuration = context.Configuration;
                return new();
            })
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(configuration);
        Assert.Equal(value, configuration![key]);
    }

    [Fact]
    public void ConfigureHostConfiguration_ReturnsBuilder()
    {
        var builder = _builder.ConfigureHostConfiguration((_, _) => { });

        Assert.Same(_builder, builder);
    }

    [Fact]
    public async Task ConfigureServices_ConfiguresService()
    {
        var service = new object();
        IServiceProvider? services = null;
        var handler = _builder
            .ConfigureServices((_, s) => s.AddSingleton(service))
            .ConfigureHandler(context => {
                services = context.Services;
                return new();
            })
            .Build();

        await handler.InvokeAsync(_invocationContext);

        Assert.NotNull(services);
        Assert.Same(service, services!.GetService(typeof(object)));
    }

    [Fact]
    public void ConfigureServices_ReturnsBuilder()
    {
        var builder = _builder.ConfigureServices((_, _) => { });

        Assert.Same(_builder, builder);
    }

    [Fact]
    public void Create_CreatesNewHandlerBuilder()
    {
        var result = HandlerBuilder.Create();

        Assert.IsType<HandlerBuilder>(result);
    }
}
