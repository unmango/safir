// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Cli
{
    internal record Options
    {
        public Config? Config { get; init; }
    }

    internal record Config
    {
        public string File { get; init; } = string.Empty;

        public bool Exists { get; init; }
    }
}
