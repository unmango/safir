using System.Collections.Generic;

namespace Safir.Common.State
{
    internal class On<TState, TAction> : IOn<TState, TAction>
            where TAction : IAction
    {
        public On(Reducer<TState, TAction> reducer, params string[] types)
        {
            Reducer = reducer;
            Types = types;
        }

        public Reducer<TState, TAction> Reducer { get; }

        public IEnumerable<string> Types { get; }
    }
}
