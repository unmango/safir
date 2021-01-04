using System.Collections.Immutable;

namespace Cli.Services
{
    internal record InstallationContext
    {
        private ImmutableDictionary<object, object> Properties { get; init; } =
            ImmutableDictionary<object, object>.Empty;
    }
}
