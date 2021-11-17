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
        public static IServiceCollection AddEntityFrameworkEventSourcing(this IServiceCollection services)
        {
            services.AddEventSourcing();
            services.AddDbContextEventStore();
            services.AddDbContextSnapshotStore();

            return services;
        }

        public static IServiceCollection AddEntityFrameworkEventSourcing<T>(this IServiceCollection services)
            where T : DbContext
        {
            services.AddEventSourcing();
            services.AddDbContextEventStore<T>();
            services.AddDbContextSnapshotStore<T>();

            return services;
        }

        public static IServiceCollection AddDbContextEventStore(this IServiceCollection services)
            => services.AddDbContextEventStore(_ => { });

        public static IServiceCollection AddDbContextEventStore(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configure)
        {
            services.AddDbContext<EventSourcingContext>(configure);
            services.AddDbContextEventStore<EventSourcingContext>();

            return services;
        }

        // TODO: We assume the consumer has already added the DbContext so it can be configured correctly.
        // This may need to be revisited, because we require it in the event store and it may not exist.
        public static IServiceCollection AddDbContextEventStore<T>(this IServiceCollection services)
            where T : DbContext
        {
            services.AddScoped<IEventStore, DbContextEventStore<T>>();
            services.AddScoped<IEventStore<Guid>, DbContextEventStore<T>>();

            return services;
        }

        public static IServiceCollection AddDbContextSnapshotStore(this IServiceCollection services)
            => services.AddDbContextSnapshotStore(_ => { });

        public static IServiceCollection AddDbContextSnapshotStore(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configure)
        {
            services.AddDbContext<EventSourcingContext>(configure);
            services.AddDbContextSnapshotStore<EventSourcingContext>();

            return services;
        }

        // TODO: We assume the consumer has already added the DbContext so it can be configured correctly.
        // This may need to be revisited, because we require it in the event store and it may not exist.
        public static IServiceCollection AddDbContextSnapshotStore<T>(this IServiceCollection services)
            where T : DbContext
        {
            services.AddScoped<ISnapshotStore, DbContextSnapshotStore<T>>();

            return services;
        }
    }
}
