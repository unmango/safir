using Microsoft.Extensions.Options;
using Safir.Redis.Configuration;

namespace Safir.Messaging.Configuration
{
    internal sealed class ConfigureRedisOptions : IConfigureOptions<RedisOptions>
    {
        private readonly IOptions<MessagingOptions> _options;

        public ConfigureRedisOptions(IOptions<MessagingOptions> options)
        {
            _options = options;
        }
        
        public void Configure(RedisOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.Configuration)) return;

            options.Configuration = _options.Value.ConnectionString;
        }
    }
}
