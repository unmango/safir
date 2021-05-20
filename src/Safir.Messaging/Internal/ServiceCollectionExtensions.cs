using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Safir.Common;

namespace Safir.Messaging.Internal
{
    internal static class ServiceCollectionExtensions
    {
        public static IEnumerable<Type> GetRegisteredEventTypes(this IServiceCollection services)
        {
            return services.Select(x => x.ImplementationType)
                .Where(IsGenericHandler)
                .GroupBy(GetEventType)
                .Select(x => x.Key);
        }

        private static bool IsGenericHandler(Type? type)
        {
            return type?.IsAssignableToGeneric(typeof(IEventHandler<>)) ?? false;
        }

        private static Type GetEventType(Type? type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetInterfaces()
                .First(x => x.IsGenericType)
                .GetGenericArguments()
                .First();
        }
    }
}
