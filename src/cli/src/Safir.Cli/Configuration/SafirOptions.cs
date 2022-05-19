namespace Safir.Cli.Configuration;

internal record SafirOptions
{
    public ConfigOptions Config { get; init; } = new();
}

internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public string File { get; init; } = string.Empty;
}
