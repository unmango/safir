using System.Collections.Generic;
using System.Linq;

namespace Cli.Services
{
    internal static class ServiceStatusExtensions
    {
        public static IEnumerable<ServiceStatus> GetStatus(
            this IServiceStatus serviceStatus,
            params IService[] services)
            => GetStatus(serviceStatus, (IEnumerable<IService>)services);
        
        public static IEnumerable<ServiceStatus> GetStatus(
            this IServiceStatus serviceStatus,
            IEnumerable<IService> services)
            => services.Select(serviceStatus.GetStatus);
    }
}
