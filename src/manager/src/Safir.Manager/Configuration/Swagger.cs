using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Safir.Manager.Configuration
{
    internal class Swagger :
        IConfigureOptions<SwaggerGenOptions>,
        IConfigureOptions<SwaggerUIOptions>
    {
        private const string Title = "Safir Manager";
        
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = Title,
                Version = "v1"
            });
        }

        public void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Title} V1");
        }
    }
}
