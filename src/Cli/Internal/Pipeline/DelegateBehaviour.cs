using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal class DelegateBehaviour<T> : IPipelineBehaviour<T>
        where T : class
    {
        private readonly bool _appliesToResult;
        private readonly AppliesTo<T>? _appliesTo;
        private readonly InvokeAsync<T>? _invokeAsync;

        public DelegateBehaviour(InvokeAsync<T> invokeAsync, bool appliesToResult = false)
            : this(null, invokeAsync)
        {
            _appliesToResult = appliesToResult;
        }
        
        public DelegateBehaviour(AppliesTo<T>? appliesTo, InvokeAsync<T>? invokeAsync)
        {
            _appliesTo = appliesTo;
            _invokeAsync = invokeAsync;
        }
        
        public bool AppliesTo(T context)
        {
            return _appliesTo?.Invoke(context) ?? _appliesToResult;
        }

        public ValueTask InvokeAsync(T context, Func<T, ValueTask> next, CancellationToken cancellationToken = default)
        {
            return _invokeAsync?.Invoke(context, next, cancellationToken) ?? ValueTask.CompletedTask;
        }
    }
}
