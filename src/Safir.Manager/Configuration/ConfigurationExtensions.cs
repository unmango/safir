using Microsoft.Extensions.Configuration;

namespace Safir.Manager.Configuration
{
    internal static class ConfigurationExtensions
    {
        public static bool IsSelfContained(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("SelfContained");
        }
    }
}
