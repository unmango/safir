using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.Commands;
using Xunit;

namespace Safir.Cli.Tests.Commands;

public class CommandBuilderTests
{
    private readonly CommandBuilder _builder = CommandBuilder.Create();

    [Fact]
    public void Build_UsesConfiguredService()
    {
        _builder.ConfigureServices((_, services) => {
            services.AddSingleton<FakeService>();
        });

        var services = _builder.Build();

        Assert.Contains(services, x => x.ServiceType == typeof(FakeService));
    }

    [Fact]
    public void Build_AddsConfiguration()
    {
        _builder.Configure(_ => { });

        var services = _builder.Build();

        Assert.Contains(services, x => x.ServiceType == typeof(IConfiguration));
    }

    private record FakeHandler;

    private record FakeService;
}
