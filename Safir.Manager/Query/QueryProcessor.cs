using SimpleInjector;
using System.Diagnostics;

namespace Safir.Manager.Query
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly Container _container;

        public QueryProcessor(Container container) {
            _container = container;
        }

        [DebuggerStepThrough]
        public TResult Process<TResult>(IQuery<TResult> query) {
            var handlerType =
                typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _container.GetInstance(handlerType);
            return handler.Execute((dynamic)query);
        }
    }
}
