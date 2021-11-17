using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Services.Configuration
{
    internal static class ServiceSourceExtensions
    {
        public static IEnumerable<T> OrderByPriority<T>(this IEnumerable<T> sources)
            where T : IServiceSource
        {
            // TODO: Logic for putting entries with 'unset' priority last when converting to concrete entry
            return sources.OrderByDescending(x => x.Priority == default).ThenBy(x => x.Priority);
        }

        public static T HighestPriority<T>(
            this IEnumerable<T> sources,
            Func<T, bool>? predicate = null)
            where T : IServiceSource
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.First()
                : ordered.First(predicate);
        }

        public static T? HighestPriorityOrDefault<T>(
            this IEnumerable<T> sources,
            Func<T, bool>? predicate = null)
            where T : IServiceSource
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
