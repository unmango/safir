using Microsoft.Extensions.DependencyInjection;
using Safir.Common.State;
using Safir.FileManager.Store;

namespace Safir.FileManager.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManagerInfrastructure(this IServiceCollection services)
        {
            var store = new Store(InitialState.Value, (action, state) => state);

            services.AddSingleton<IStore<State>>(store);

            return services;
        }
    }
}
