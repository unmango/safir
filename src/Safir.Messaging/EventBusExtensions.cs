using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
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
                return bus.SubscribeAsync(syncObservable);
            });
        }

        public static IObservable<T> ObserveWith<T>(this IEventBus bus, IEventHandler<T> handler)
            where T : IEvent
        {
            return bus.GetObservable<T>().ObserveWith(handler);
        }

        public static IDisposable Subscribe<T>(this IEventBus bus, Action<T> callback)
            where T : IEvent
        {
            return bus.GetObservable<T>().Subscribe(callback);
        }

        public static IDisposable Subscribe<T>(this IEventBus bus, IEventHandler<T> handler)
            where T : IEvent
        {
            return bus.ObserveWith(handler).Subscribe();
        }

        public static IEnumerable<IDisposable> Subscribe<T>(
            this IEventBus bus,
            IEnumerable<IEventHandler<T>> handlers)
            where T : IEvent
        {
            return bus.Subscribe(typeof(T), handlers);
        }

        public static IDisposable SubscribeRetry<T>(this IEventBus bus, IEventHandler<T> handler, Action<Exception> onError)
            where T : IEvent
        {
            return bus.ObserveWith(handler).Retry().SubscribeSafe(onError);
        }

        public static IDisposable SubscribeSafe<T>(this IEventBus bus, IObserver<T> observer)
            where T : IEvent
        {
            return bus.GetObservable<T>().SubscribeSafe(observer);
        }

        public static IDisposable SubscribeSafe<T>(this IEventBus bus, IEventHandler<T> handler, Action<Exception> onError)
            where T : IEvent
        {
            return bus.ObserveWith(handler).SubscribeSafe(onError);
        }

        internal static IDisposable Subscribe(this IEventBus bus, Type eventType, IEventHandler handler)
        {
            return SubscribeSafe(bus, eventType, handler, _ => { });
        }

        internal static IEnumerable<IDisposable> Subscribe(
            this IEventBus bus,
            Type eventType,
            IEnumerable<IEventHandler> handlers)
        {
            return SubscribeSafe(bus, eventType, handlers, _ => { });
        }

        internal static IEnumerable<IDisposable> SubscribeRetry(
            this IEventBus bus,
            Type eventType,
            IEnumerable<IEventHandler> handlers,
            Action<Exception> onError)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.SubscribeRetry(bus, handlers, onError);
        }

        internal static IDisposable SubscribeRetry(
            this IEventBus bus,
            Type eventType,
            IEventHandler handler,
            Action<Exception> onError)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.SubscribeRetry(bus, handler, onError);
        }

        internal static IDisposable SubscribeSafe(
            this IEventBus bus,
            Type eventType,
            IEventHandler handler,
            Action<Exception> onError)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.SubscribeSafe(bus, handler, onError);
        }

        internal static IEnumerable<IDisposable> SubscribeSafe(
            this IEventBus bus,
            Type eventType,
            IEnumerable<IEventHandler> handlers,
            Action<Exception> onError)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("eventType is not assignable to IEvent");
            }

            var wrapperType = typeof(SubscribeHandlerWrapper<>).MakeGenericType(eventType);
            var wrapped = (ISubscribeHandlerWrapper)Activator.CreateInstance(wrapperType);
            return wrapped.SubscribeSafe(bus, handlers, onError);
        }
    }
}
