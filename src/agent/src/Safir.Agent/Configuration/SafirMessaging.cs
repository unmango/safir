using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Safir.Messaging.Configuration;

namespace Safir.Agent.Configuration
{
    internal class SafirMessaging : IConfigureOptions<MessagingOptions>
    {
        private readonly AgentOptions _options;

        public SafirMessaging(IConfiguration configuration)
        {
            _options = configuration.Get<AgentOptions>();
        }

        public void Configure(MessagingOptions options)
        {
            options.ConnectionString = _options.Redis;
        }
    }
}
