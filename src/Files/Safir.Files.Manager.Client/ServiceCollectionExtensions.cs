using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManagerClient(this IServiceCollection services)
        {
            services.AddHttpClient();

            throw new NotImplementedException();
        }
    }
}
