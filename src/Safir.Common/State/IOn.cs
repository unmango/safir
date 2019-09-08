using System.Collections.Generic;

namespace Safir.Common.State
{
    public interface IOn<TState, TAction>
        where TAction : IAction
    {
        Reducer<TState, TAction> Reducer { get; }

        IEnumerable<string> Types { get; }
    }
}
