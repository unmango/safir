using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal static class PipelineBehaviourExtensions
    {
        public static IPipeline<T> BuildPipeline<T>(this IEnumerable<IPipelineBehaviour<T>> behaviours)
            where T : class
            => new BehaviourPipeline<T>(behaviours);
        
        public static InvokeAsync<T> GetPipelineDelegate<T>(this IEnumerable<IPipelineBehaviour<T>> behaviours)
            where T : class
            => behaviours.Select(GetInvokeDelegate).GetPipelineDelegate();
        
        public static InvokeAsync<T> GetPipelineDelegate<T>(this IEnumerable<InvokeAsync<T>> invocations)
            where T : class
            => invocations.Aggregate(
                (first, second)
                    => (context, next, cancellationToken)
                        => first(
                            context,
                            innerContext => second(innerContext, next, cancellationToken),
                            cancellationToken));
        
        public static InvokeAsync<T> GetInvokeDelegate<T>(this IPipelineBehaviour<T> behaviour)
            where T : class
            => behaviour.InvokeAsync;

        public static ValueTask InvokePipelineAsync<T>(
            this IEnumerable<IPipelineBehaviour<T>> behaviours,
            T context,
            CancellationToken cancellationToken = default)
            where T : class
            => behaviours.GetPipelineDelegate().InvokeAsync(context, cancellationToken);

        public static ValueTask InvokePipelineAsync<T>(
            this IEnumerable<IPipelineBehaviour<T>> behaviours,
            T context,
            Func<T, ValueTask> next,
            CancellationToken cancellationToken = default)
            where T : class
            => behaviours.GetPipelineDelegate().InvokeAsync(context, next, cancellationToken);
    }
}
