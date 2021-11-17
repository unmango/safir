using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Safir.Messaging.Configuration;

namespace Safir.Manager.Configuration
{
    internal class SafirMessaging : IConfigureOptions<MessagingOptions>
    {
        private readonly ManagerOptions _options;

        public SafirMessaging(IConfiguration configuration)
        {
            _options = configuration.Get<ManagerOptions>();
        }

        public void Configure(MessagingOptions options)
        {
            options.ConnectionString = _options.Redis;
        }
    }
}
