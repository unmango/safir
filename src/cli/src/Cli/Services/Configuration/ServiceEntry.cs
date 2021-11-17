using System.Collections.Generic;

namespace Cli.Services.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal record ServiceEntry
    {
        public string? Cwd { get; init; }

        public string? Name { get; init; }

        // Because of jank with the Microsoft.Extensions.Options API
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IReadOnlyList<ServiceSource> Sources { get; init; } = null!;
    }
}
