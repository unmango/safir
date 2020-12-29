using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal static class ServiceRegistryExtensions
    {
        public static async IAsyncEnumerable<IService> GetServices(this IServiceRegistry registry, IEnumerable<string> names)
        {
            var nameList = names.ToList();
            var unsupported = nameList.Except(registry.Services.Select(x => x.Name)).ToList();
            if (unsupported.Any())
                throw new NotSupportedException($"Unrecognized services: {string.Join(',', unsupported)}");

            foreach (var name in nameList)
            {
                yield return await registry.GetServiceAsync(name);
            }
        }

        public static Task<IService> GetServiceAsync(this IServiceRegistry registry, string name)
        {
            throw new NotImplementedException();
        }
    }
}
