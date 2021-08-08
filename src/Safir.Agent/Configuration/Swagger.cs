using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Safir.Agent.Configuration
{
    internal class Swagger :
        IConfigureOptions<SwaggerGenOptions>,
        IConfigureOptions<SwaggerUIOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = "Safir Agent",
                Version = "v1"
            });
        }

        public void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Safir Agent V1");
        }
    }
}
