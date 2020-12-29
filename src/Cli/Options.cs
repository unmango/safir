// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;
using Cli.Services;

namespace Cli
{
    internal record Options
    {
        public Config Config { get; init; } = new();

        public Service Service { get; init; } = new();
    }

    internal record Config
    {
        public string Directory { get; init; } = string.Empty;

        public bool Exists { get; init; }
        
        public string File { get; init; } = string.Empty;
    }

    internal class Service : Dictionary<string, ServiceEntry>
    {
        public const string Directory = "services";
        
        public string? CustomDirectory { get; init; }
    }
}
