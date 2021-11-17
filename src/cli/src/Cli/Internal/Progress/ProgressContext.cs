using System.Collections.Immutable;

namespace Cli.Internal.Progress
{
    internal record ProgressContext(object Value)
    {
        public IImmutableDictionary<object, object> Properties { get; } = ImmutableDictionary<object, object>.Empty;
        
        public ProgressScope Scope { get; init; } = ProgressScope.Empty;
    }
}
