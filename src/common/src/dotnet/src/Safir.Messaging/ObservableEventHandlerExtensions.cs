using System;
using System.Reactive.Linq;

namespace Safir.Messaging
{
    public static class ObservableEventHandlerExtensions
    {
        public static IObservable<T> ObserveWith<T>(this IObservable<T> observable, IEventHandler<T> handler)
            where T : IEvent
        {
            return observable.SelectMany(async (message, cancellationToken) => {
                await handler.HandleAsync(message, cancellationToken);
                return message;
            });
        }
    }
}
