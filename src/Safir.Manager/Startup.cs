using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Safir.Agent.Client.DependencyInjection;
using Safir.Manager.Agents;
using Safir.Manager.Configuration;
using Safir.Manager.Events;
using Safir.Manager.Services;
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
            services.AddGrpcHttpApi();
            services.AddGrpcSwagger();

            services.AddSwaggerGen();

            services.AddSafirMessaging();

            services.Configure<ManagerOptions>(Configuration);
            services.ConfigureOptions<SafirMessaging>();
            services.ConfigureOptions<Swagger>();

            services.AddSingleton<AgentFactory>();
            var managerOptions = Configuration.Get<ManagerOptions>();
            foreach (var agent in managerOptions.Agents)
            {
                var name = agent.BaseUrl;
                services.AddSafirAgentClient(name, options => {
                    options.Address = new Uri(agent.BaseUrl);
                });

                services.AddTransient(
                    s => s.GetRequiredService<AgentFactory>().Create(name));
            }

            services.AddTransient<DefaultAgentAggregator>();
            services.AddEventHandler<FileCreatedHandler>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            var options = app.ApplicationServices
                .GetRequiredService<IOptions<ManagerOptions>>()
                .Value;

            if (env.IsDevelopment() || options.EnableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGrpcService<MediaService>();
                
                if (env.IsDevelopment() || options.EnableGrpcReflection)
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGet("/", context => {
                    context.Response.Redirect("/swagger/index.html");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
