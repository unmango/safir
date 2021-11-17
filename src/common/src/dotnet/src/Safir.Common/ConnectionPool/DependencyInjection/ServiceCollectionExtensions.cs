using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Safir.Common.ConnectionPool.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConnectionPool<T>(
            this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task<T>> connect,
            Action<ConnectionPoolOptions<T>> configure)
        {
            services.AddConnectionPool();
            services.Configure(configure);

            services.AddTransient<ICreateConnection<T>>(sp => {
                return new CreateConnection<T>(cancellationToken => connect(sp, cancellationToken));
            });

            return services;
        }

        public static IServiceCollection AddConnectionPool<T>(
            this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task<T>> connect)
            => services.AddConnectionPool(connect, _ => { });
        
        public static IServiceCollection AddConnectionPool<TConnection, TCreate>(
            this IServiceCollection services,
            Action<ConnectionPoolOptions<TConnection>> configure)
            where TCreate : class, ICreateConnection<TConnection>
        {
            services.AddConnectionPool();
            services.Configure(configure);
            services.AddTransient<ICreateConnection<TConnection>, TCreate>();

            return services;
        }

        public static IServiceCollection AddConnectionPool<TConnection, TCreate>(this IServiceCollection services)
            where TCreate : class, ICreateConnection<TConnection>
            => services.AddConnectionPool<TConnection, TCreate>(_ => { });

        private static void AddConnectionPool(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            
            services.TryAddSingleton(typeof(IConnectionPool<>), typeof(DefaultConnectionPool<>));
            services.TryAddTransient(typeof(IConnectionManager<>), typeof(DefaultConnectionManager<>));
            services.TryAddTransient(typeof(IDisposeConnection<>), typeof(DefaultDisposeConnection<>));
        }
    }
}
