using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Services
{
    internal static class ServiceSourceExtensions
    {
        public static IEnumerable<ServiceSource> OrderByPriority(this IEnumerable<ServiceSource> sources)
        {
            return sources.OrderByDescending(x => x.Priority.HasValue).ThenBy(x => x.Priority);
        }

        public static ServiceSource HighestPriority(
            this IEnumerable<ServiceSource> sources,
            Func<ServiceSource, bool>? predicate = null)
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.First()
                : ordered.First(predicate);
        }

        public static ServiceSource? HighestPriorityOrDefault(
            this IEnumerable<ServiceSource> sources,
            Func<ServiceSource, bool>? predicate = null)
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.FirstOrDefault()
                : ordered.FirstOrDefault(predicate);
        }

        public static SourceType? InferSourceType(this ServiceSource source)
            => source switch {
                { Type: not null } => source.Type,
                { BuildContext: not null } => SourceType.DockerBuild,
                { ImageName: not null } => SourceType.DockerImage,
                { CloneUrl: not null } => SourceType.Git,
                _ => null,
            };

        public static SourceType? InferSourceType(this ServiceSource source, out ServiceSource updated)
        {
            var inferred = source.InferSourceType();
            updated = source with { Type = inferred };
            return inferred;
        }

        public static bool TryInferSourceType(this ServiceSource source, out SourceType type)
        {
            var inferred = source.InferSourceType();
            type = inferred.GetValueOrDefault();
            return inferred.HasValue;
        }
    }
}
