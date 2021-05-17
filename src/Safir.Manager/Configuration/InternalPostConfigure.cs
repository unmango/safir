using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Safir.Manager.Configuration
{
    internal class InternalPostConfigure : IPostConfigureOptions<ManagerOptions>
    {
        private readonly IConfiguration _configuration;

        public InternalPostConfigure(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void PostConfigure(string name, ManagerOptions options)
        {
            options.IsSelfContained = _configuration.IsSelfContained();
        }
    }
}
