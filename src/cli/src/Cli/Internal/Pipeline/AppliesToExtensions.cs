using System.Collections.Generic;
using System.Linq;

namespace Cli.Internal.Pipeline
{
    internal static class AppliesToExtensions
    {
        public static IEnumerable<TAppliesTo> AllApplicable<TAppliesTo, TContext>(
            this IEnumerable<TAppliesTo> enumerable,
            TContext context)
            where TAppliesTo : IAppliesTo<TContext>
            => enumerable.Where(x => x.AppliesTo(context));
        
        public static AppliesTo<T> GetAppliesToDelegate<T>(this IAppliesTo<T> behaviour)
            => behaviour.AppliesTo;
    }
}
