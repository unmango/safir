using System.Threading.Tasks;
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
            services.AddGrpcHttpApi();
            services.AddGrpcReflection();
            
            services.AddSwaggerGen();
            services.AddGrpcSwagger();
            services.ConfigureOptions<Swagger>();
            
            services.AddCors();
            services.ConfigureOptions<Cors>();

            services.AddMediatR(typeof(Startup));
            
            services.AddSafirMessaging();
            services.ConfigureOptions<SafirMessaging>();
            
            services.Configure<AgentOptions>(Configuration);
            services.ConfigureOptions<ReplaceEnvironmentVariables>();
            services.ConfigureOptions<ReplaceUnderscores>();

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
            // app.UseHttpsRedirection();

            var options = app.ApplicationServices
                .GetRequiredService<IOptions<AgentOptions>>()
                .Value;

            if (env.IsDevelopment() || options.EnableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseRouting();
            app.UseGrpcWeb(new() { DefaultEnabled = true });
            app.UseCors();
            app.UseEndpoints(endpoints => {
                endpoints.MapGrpcService<FileSystemService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<HostService>().RequireCors("AllowAll");

                if (env.IsDevelopment() || options.EnableGrpcReflection)
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGet("/config", async context => {
                    await context.Response.WriteAsJsonAsync(options);
                });

                endpoints.MapGet("/", context => {
                    context.Response.Redirect("/swagger/index.html");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
