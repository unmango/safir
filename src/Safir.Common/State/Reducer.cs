using System;
using System.Collections.Generic;

namespace Safir.Common.State
{
    public delegate TState Reducer<TState, in TAction>(TState state, TAction action)
        where TAction : IAction;

    public static class Reducer
    {
        public static Reducer<TState, TAction> Create<TState, TAction>(
            TState initialState,
            params IOn<TState, TAction>[] ons)
            where TAction : IAction
        {
            var map = new Dictionary<string, Reducer<TState, TAction>>();

            foreach (var on in ons)
            {
                foreach (var type in on.Types)
                {
                    map.Add(type, on.Reducer);
                }
            }

            return (state, action) =>
            {
                state ??= initialState;

                if (map.TryGetValue(action.Type, out var reducer))
                {
                    return reducer(state, action);
                }

                return state;
            };
        }

        public static IOn<TState, TAction> On<TState, TAction>(TAction action, Func<TState, TState> reducer)
            where TAction : IAction
        {
            return new On<TState, TAction>((state, action) => reducer(state), action.Type);
        }
    }
}
