using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Safir.Common;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventHandlerExtensions
    {
        public static IEnumerable<Type> GetEventTypes(this IEventHandler handler)
        {
            return handler.GetType()
                .GetInterfaces()
                .Where(IsGenericHandler)
                .Select(x => x.GetGenericArguments()[0]);
        }

        public static IEnumerable<Type> GetEventTypes(this IEnumerable<IEventHandler> handlers)
        {
            return handlers.SelectMany(GetEventTypes).Distinct();
        }
        
        public static IEnumerable<IGrouping<Type, IEventHandler>> GroupByEvent(this IEnumerable<IEventHandler> handlers)
        {
            return handlers.Where(IsGenericHandler)
                .SelectMany(x => x.GetEventTypes(), (handler, type) => new { handler, type })
                .GroupBy(x => x.type, x => x.handler);
        }

        public static bool IsGenericHandler(this IEventHandler handler)
        {
            return handler.GetType().IsGenericHandler();
        }

        public static IDisposable Subscribe<T>(this IEventHandler<T> handler, IEventBus bus)
            where T : IEvent
        {
            return bus.Subscribe(handler);
        }

        private static bool IsGenericHandler(this Type type)
        {
            return type.IsAssignableToGeneric(typeof(IEventHandler<>));
        }
    }
}
