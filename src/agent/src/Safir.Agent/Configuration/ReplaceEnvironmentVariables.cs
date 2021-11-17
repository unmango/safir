using System;
using Microsoft.Extensions.Options;

namespace Safir.Agent.Configuration
{
    internal sealed class ReplaceEnvironmentVariables : IPostConfigureOptions<AgentOptions>
    {
        public void PostConfigure(string name, AgentOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.DataDirectory))
            {
                options.DataDirectory = Environment.ExpandEnvironmentVariables(options.DataDirectory);
            }
        }
    }
}
