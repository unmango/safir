using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable StringLiteralTypo

namespace Safir.Agent.Configuration
{
    internal class ReplaceUnderscores : IPostConfigureOptions<AgentOptions>
    {
        private readonly IConfiguration _configuration;

        public ReplaceUnderscores(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PostConfigure(string name, AgentOptions options)
        {
            if (!string.IsNullOrEmpty(_configuration["ENABLE_GRPCREFLECTION"]))
            {
                options.EnableGrpcReflection = _configuration.GetValue<bool>("ENABLE_GRPCREFLECTION");
            }

            if (!string.IsNullOrEmpty(_configuration["ENABLE_GRPC_REFLECTION"]))
            {
                options.EnableGrpcReflection = _configuration.GetValue<bool>("ENABLE_GRPC_REFLECTION");
            }

            if (!string.IsNullOrEmpty(_configuration["ENABLE_SWAGGER"]))
            {
                options.EnableSwagger = _configuration.GetValue<bool>("ENABLE_SWAGGER");
            }
        }
    }
}
