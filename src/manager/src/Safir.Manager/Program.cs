using Safir.Agent.Client.DependencyInjection;
using Safir.Manager;
using Safir.Manager.Services;
using Serilog;

const string title = "Safir Manager";
const string version = "v1";
const string corsAllowAllPolicy = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(static (context, services, configuration) => configuration
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console(outputTemplate: "[{SourceContext:1} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

var services = builder.Services;

// gRPC
services.AddGrpc();

if (builder.Environment.IsDevelopment()) {
    services.AddGrpcReflection();
    services.AddGrpcSwagger();
    services.AddSwaggerGen(static options => {
        options.SwaggerDoc(version, new() {
            Title = title,
            Version = version,
        });
    });
}

// Agent
var agentOptions = builder.Configuration
    .Get<ManagerConfiguration>()
    .GetAgentOptions();

services.AddSafirAgentClient();
foreach (var agent in agentOptions) {
    services.AddSafirAgentClient(agent.Name, options => {
        options.Address = agent.Uri;
    });
}

services.AddTransient<IAgents, AgentAggregator>();

// Other
services.Configure<ManagerConfiguration>(builder.Configuration);
services.AddCors(static options => {
    options.AddPolicy(corsAllowAllPolicy, static builder => {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});

// App
var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(static options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} {version}");
    });
}

app.UseGrpcWeb(new() { DefaultEnabled = true });
app.UseCors();

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

app.MapGrpcService<HostService>().RequireCors(corsAllowAllPolicy);
app.MapGrpcService<MediaService>().RequireCors(corsAllowAllPolicy);

app.Run();

// Make Program `public` for testing. Yuck
public partial class Program { }
