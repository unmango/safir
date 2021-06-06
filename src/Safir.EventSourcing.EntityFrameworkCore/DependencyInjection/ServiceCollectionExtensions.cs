using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Safir.EventSourcing.DependencyInjection;

namespace Safir.EventSourcing.EntityFrameworkCore.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventDbContext(this IServiceCollection services)
            => services.AddEventDbContext(_ => { });

        public static IServiceCollection AddEventDbContext(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configure)
        {
            services.AddDbContext<EventDbContext>(configure);
            services.AddEventDbContext<EventDbContext>();

            return services;
        }

        public static IServiceCollection AddEventDbContext<T>(this IServiceCollection services)
            where T : DbContext
        {
            services.AddEventSourcing();
            services.AddScoped<IEventStore, DbContextEventStore<T>>();
            services.AddScoped<IEventStore<Guid>, DbContextEventStore<T>>();
            services.AddScoped<IEventStore<Guid, Guid>, DbContextEventStore<T>>();

            return services;
        }
    }
}
