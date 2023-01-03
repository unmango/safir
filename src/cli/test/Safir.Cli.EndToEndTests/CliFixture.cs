using DotNet.Testcontainers.Images;
using JetBrains.Annotations;
using Safir.EndToEndTesting;

namespace Safir.Cli.EndToEndTests;

[UsedImplicitly]
public sealed class CliFixture : SafirFixture
{
    private const string TagPrefix = "cli";

    public CliFixture() : base(TagPrefix)
    {
        CliImage = Image(SafirImageBuilder.DefaultCliImageName, TagPrefix);
    }

    public IDockerImage CliImage { get; }

    public override Task InitializeAsync()
    {
        var cliBuild = SafirImageBuilder.Create()
            .WithSafirCliConfiguration()
            .WithName(CliImage)
            .Build();

        return Task.WhenAll(cliBuild, base.InitializeAsync());
    }
}
