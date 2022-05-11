// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Safir.Cli;

internal record CliOptions
{
    public ConfigOptions Config { get; init; } = new();
}

internal record ConfigOptions
{
    public string Directory { get; init; } = string.Empty;

    public bool Exists { get; init; }

    public string File { get; init; } = string.Empty;
}
