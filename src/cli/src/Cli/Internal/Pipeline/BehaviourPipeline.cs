using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal class BehaviourPipeline<T> : IPipeline<T>
        where T : class
    {
        private readonly IEnumerable<IPipelineBehaviour<T>> _behaviours;

        public BehaviourPipeline(IEnumerable<IPipelineBehaviour<T>> behaviours)
        {
            _behaviours = behaviours ?? throw new ArgumentNullException(nameof(behaviours));
        }
        
        public ValueTask InvokeAsync(T context, CancellationToken cancellationToken)
        {
            return _behaviours.GetPipelineDelegate().InvokeAsync(context, cancellationToken);
        }
    }
}
