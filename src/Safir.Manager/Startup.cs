using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Safir.Agent.Client.DependencyInjection;
using Safir.Manager.Events;
using Safir.Messaging.DependencyInjection;
using Serilog;

namespace Safir.Manager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        private IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddGrpcReflection();

            services.AddSafirAgentClient();
            services.AddSafirMessaging(options => {
                options.ConnectionString = Configuration["Redis"];
            });
            services.AddEventHandler<FileCreatedHandler>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }
                
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints must be made through a gRPC client");
                });
            });
        }
    }
}
