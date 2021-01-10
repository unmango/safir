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
        
        public static IPipelineBehaviour<T> Decorate<T>(
            this IPipelineBehaviour<T> behaviour,
            IPipelineBehaviour<T> decorator)
            where T : class
            => new BehaviourDecorator<T>(behaviour, decorator);

        public static IPipelineBehaviour<T> Decorate<T>(
            this IPipelineBehaviour<T> behaviour,
            InvokeAsync<T> decorator)
            where T : class
            => behaviour.Decorate(new DelegateBehaviour<T>(decorator));

        public static IPipelineBehaviour<T> Decorate<T>(
            this IPipelineBehaviour<T> behaviour,
            AppliesTo<T> appliesTo,
            InvokeAsync<T> decorator)
            where T : class
            => behaviour.Decorate(new DelegateBehaviour<T>(appliesTo, decorator));

        public static Func<T, ValueTask> GetNextDelegate<T>(
            this IPipelineBehaviour<T> behaviour,
            CancellationToken cancellationToken = default)
            where T : class
            => behaviour.GetInvokeDelegate().GetNextDelegate(cancellationToken);

        public static Func<T, ValueTask> GetNextDelegate<T>(
            this IPipelineBehaviour<T> behaviour,
            Func<T, ValueTask> next,
            CancellationToken cancellationToken = default)
            where T : class
            => behaviour.GetInvokeDelegate().GetNextDelegate(next, cancellationToken);

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

        public static Func<T, ValueTask> GetNextDelegate<T>(
            this InvokeAsync<T> invokeAsync,
            CancellationToken cancellationToken = default)
            => invokeAsync.GetNextDelegate(_ => ValueTask.CompletedTask, cancellationToken);

        public static Func<T, ValueTask> GetNextDelegate<T>(
            this InvokeAsync<T> invokeAsync,
            Func<T, ValueTask> next,
            CancellationToken cancellationToken = default)
            => context => invokeAsync(context, next, cancellationToken);

        private static AppliesTo<T> GetAppliesToDelegate<T>(this IPipelineBehaviour<T> behaviour)
            where T : class
            => behaviour.AppliesTo;

        private static InvokeAsync<T> GetInvokeDelegate<T>(this IPipelineBehaviour<T> behaviour)
            where T : class
            => behaviour.InvokeAsync;
    }
}
