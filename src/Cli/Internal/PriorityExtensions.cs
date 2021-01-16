using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Internal
{
    internal static class PriorityExtensions
    {
        public static IEnumerable<T> OrderByPriority<T>(this IEnumerable<T> sources)
            where T : IPriority
        {
            // TODO: Logic for putting entries with 'unset' priority last when converting to concrete entry
            return sources.OrderByDescending(x => x.Priority == default).ThenBy(x => x.Priority);
        }

        public static T HighestPriority<T>(
            this IEnumerable<T> sources,
            Func<T, bool>? predicate = null)
            where T : IPriority
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.First()
                : ordered.First(predicate);
        }

        public static T? HighestPriorityOrDefault<T>(
            this IEnumerable<T> sources,
            Func<T, bool>? predicate = null)
            where T : IPriority
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.FirstOrDefault()
                : ordered.FirstOrDefault(predicate);
        }
    }
}
