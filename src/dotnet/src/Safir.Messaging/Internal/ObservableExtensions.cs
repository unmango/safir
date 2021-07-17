using System;
using System.Reactive;

namespace Safir.Messaging.Internal
{
    internal static class ObservableExtensions
    {
        public static IDisposable SubscribeSafe<T>(this IObservable<T> observable, Action<Exception> onError)
        {
            return observable.SubscribeSafe(Observer.Create<T>(_ => { }, onError));
        }
    }
}
