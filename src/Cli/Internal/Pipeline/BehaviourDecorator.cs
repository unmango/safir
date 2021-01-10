using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal class BehaviourDecorator<T> : IPipelineBehaviour<T>
        where T : class
    {
        private readonly IPipelineBehaviour<T> _decorated;
        private readonly IPipelineBehaviour<T> _decorator;
        private readonly bool _overrideAppliesTo;

        public BehaviourDecorator(
            IPipelineBehaviour<T> decorated,
            IPipelineBehaviour<T> decorator,
            bool overrideAppliesTo = false)
        {
            _decorated = decorated;
            _decorator = decorator;
            _overrideAppliesTo = overrideAppliesTo;
        }

        public bool AppliesTo(T context)
        {
            return _overrideAppliesTo
                ? _decorator.AppliesTo(context)
                : _decorated.AppliesTo(context);
        }

        public ValueTask InvokeAsync(T context, Func<T, ValueTask> next, CancellationToken cancellationToken = default)
        {
            // This can only short-circuit, can't skip decorated
            // Would like to be able to skip decorated w/o
            // relying on `AppliesTo`
            return _decorator.InvokeAsync(
                context,
                _decorated.GetNextDelegate(next, cancellationToken),
                cancellationToken);
        }
    }
}
