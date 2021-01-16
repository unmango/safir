using System;
using System.Collections.Generic;
using Cli.Internal;
using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record InvalidSource(ServiceSource Source, IEnumerable<string> Errors)
        : ServiceSource, IServiceSource
    {
        public InvalidSource(ServiceSource s, params string[] errors)
            : this(s, (IEnumerable<string>)errors)
        {
        }

        SourceType IServiceSource.Type => Source.Type ?? throw new InvalidOperationException("Source.Type was null");

        string IServiceSource.Name => Name ?? string.Empty;

        int IPriority.Priority => Source.Priority ?? default;
    }
}
