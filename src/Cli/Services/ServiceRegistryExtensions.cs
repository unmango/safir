using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Services
{
    internal static class ServiceRegistryExtensions
    {
        public static IService? Find(this IServiceRegistry registry, string? name)
            => registry.Where(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .FirstOrDefault();

        public static IEnumerable<IService> Find(this IServiceRegistry registry, IEnumerable<string?> names)
            => names.Select(registry.Find).Where(x => x != null)!;
    }
}
