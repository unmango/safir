using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging.Internal;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventBusExtensions
    {
        public static IObservable<T> GetObservable<T>(this IEventBus bus) where T : IEvent
        {
            return Observable.Create<T>(observable => {
                var syncObservable = Observer.Synchronize(observable);
                return bus.SubscribeAsync<T>(syncObservable.OnNext);
            });
        }

        public static IDisposable Subscribe<T>(this IEventBus bus, Action<T> callback)
            where T : IEvent
        {
            return bus.GetObservable<T>().Subscribe(callback);
        }

        public static IDisposable Subscribe<T>(this IEventBus bus, IEventHandler<T> handler)
            where T : IEvent
        {
            return bus.GetObservable<T>().SelectMany(HandleAsync).Subscribe();

            async Task<Unit> HandleAsync(T message, CancellationToken cancellationToken)
            {
                await handler.HandleAsync(message, cancellationToken);
                return Unit.Default;
            }
        }

        public static IEnumerable<IDisposable> Subscribe<T>(
            this IEventBus bus,
            IEnumerable<IEventHandler<T>> handlers)
            where T : IEvent
        {
            return bus.Subscribe(typeof(T), handlers);
        }

        internal static IDisposable Subscribe(this IEventBus bus, Type eventType, IEventHandler handler)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.Subscribe(bus, handler);
        }

        internal static IEnumerable<IDisposable> Subscribe(
            this IEventBus bus,
            Type eventType,
            IEnumerable<IEventHandler> handlers)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.Subscribe(bus, handlers);
        }
    }
}
