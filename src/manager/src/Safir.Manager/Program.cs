using Microsoft.OpenApi.Models;
using Safir.Agent.Client.DependencyInjection;
using Safir.Manager.Configuration;
using Safir.Manager.Services;
using Serilog;

const string title = "Safir Manager";
const string corsAllowAllPolicy = "AllowAll";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// gRPC
services.AddGrpc();
services.AddGrpcHttpApi();

if (builder.Environment.IsDevelopment()) {
    services.AddGrpcReflection();
    services.AddGrpcSwagger();
    services.AddSwaggerGen(static options => {
        options.SwaggerDoc("v1", new() {
            Title = title,
            Version = "v1",
        });
    });
}

// Agent
services.Configure<ManagerConfiguration>(builder.Configuration);
var agentOptions = builder.Configuration
    .Get<ManagerConfiguration>()
    .GetAgentOptions();

foreach (var agent in agentOptions) {
    services.AddSafirAgentClient(agent.Name, options => {
        options.Address = agent.Uri;
    });
}

// Other
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
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} V1");
    });
}

app.UseGrpcWeb(new() { DefaultEnabled = true });
app.UseCors();
app.MapGrpcReflectionService();
app.MapGrpcService<MediaService>().RequireCors(corsAllowAllPolicy);
app.MapGet(
    "/",
    () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
