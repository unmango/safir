using System;
using System.Collections.Generic;
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
            services.AddGrpcHttpApi();
            services.AddGrpcReflection();
            
            services.AddSwaggerGen();
            services.AddGrpcSwagger();
            services.ConfigureOptions<Swagger>();
            
            services.AddCors();
            services.ConfigureOptions<Cors>();

            services.AddSafirMessaging();
            services.ConfigureOptions<SafirMessaging>();

            services.Configure<ManagerOptions>(Configuration);
            var managerOptions = Configuration.Get<ManagerOptions>();
            if (managerOptions.ProxyAgent)
            {
                services.AddTransient<IAgents, AgentProxy>();
                services.AddTransient<IEnumerable<IAgent>, AgentProxy>();
            }
            else
            {
                services.AddSingleton<AgentFactory>();
                foreach (var agent in managerOptions.Agents)
                {
                    services.AddSafirAgentClient(agent.Name, options => {
                        options.Address = new Uri(agent.BaseUrl);
                    });
                }

                services.AddTransient<IAgents, AgentManager>();
                services.AddTransient<IEnumerable<IAgent>, AgentManager>();
            }

            services.AddTransient<AgentAggregator>();
            services.AddEventHandler<FileCreatedHandler>();
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
                .GetRequiredService<IOptions<ManagerOptions>>()
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
                endpoints.MapGrpcService<MediaService>().RequireCors("AllowAll");

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
