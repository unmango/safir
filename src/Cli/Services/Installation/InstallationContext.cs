using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cli.Services.Installation
{
    internal record InstallationContext(
        string WorkingDirectory,
        IService Service,
        IEnumerable<IServiceSource> Sources)
    {
        public Exception? Exception { get; init; }
        
        public bool Installed { get; init; }
        
        public IImmutableDictionary<object, object> Properties { get; init; } =
            ImmutableDictionary<object, object>.Empty;
    }
}
