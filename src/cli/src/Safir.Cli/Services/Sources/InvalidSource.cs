using System;
using System.Collections.Generic;
using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources;

internal record InvalidSource(
    ServiceSource Source,
    IEnumerable<string> Errors) : ServiceSource, IServiceSource
{
    public InvalidSource(ServiceSource s, params string[] errors)
        : this(s, (IEnumerable<string>)errors)
    {
    }

    SourceType IServiceSource.Type => Source.Type ?? throw new InvalidOperationException("Source.Type was null");

    string IServiceSource.Name => Name ?? string.Empty;

    int IServiceSource.Priority
    {
        get => Source.Priority ?? default;
        init => throw new InvalidOperationException("Can't modify Priority on type `InvalidSource`");
    }
}