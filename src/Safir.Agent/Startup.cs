using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;
using Safir.Agent.Services;
using Safir.Messaging.DependencyInjection;
using Serilog;

namespace Safir.Agent
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

            services.AddMediatR(typeof(Startup));
            services.AddSafirMessaging(options => {
                options.ConnectionString = Configuration["Redis"];
            });
            services.Configure<AgentOptions>(Configuration);
            services.AddTransient<IPostConfigureOptions<AgentOptions>, ReplaceEnvironmentVariables>();

            services.AddTransient<IDirectory, SystemDirectoryWrapper>();
            services.AddTransient<IFile, SystemFileWrapper>();
            services.AddTransient<IPath, SystemPathWrapper>();

            services.AddSingleton<DataDirectoryWatcher>();
            services.AddHostedService(s => s.GetRequiredService<DataDirectoryWatcher>());
            services.AddSingleton<IFileWatcher>(s => s.GetRequiredService<DataDirectoryWatcher>());

            services.AddHostedService<FileEventPublisher>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseGrpcWeb(new GrpcWebOptions {
                DefaultEnabled = true
            });

            app.UseEndpoints(endpoints => {
                endpoints.MapGrpcService<FileSystemService>();
                endpoints.MapGrpcService<HostService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGet("/config", async context => {
                    var options = context.RequestServices.GetRequiredService<IOptions<AgentOptions>>();
                    await context.Response.WriteAsJsonAsync(options.Value);
                });

                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints must be made through a gRPC client");
                });
            });
        }
    }
}
