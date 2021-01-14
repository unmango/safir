namespace Cli.Services
{
    internal record ServiceStatus(
        string Name,
        ServiceInstalledStatus Installed,
        ServiceHealthStatus Health,
        ServiceStateStatus State);

    internal record ServiceInstalledStatus(bool Installed)
    {
        public string? Location { get; init; }

        public static ServiceInstalledStatus At(string location) => new(true) { Location = location };
    }
    
    internal record ServiceHealthStatus(bool Healthy)
    {
        public static readonly ServiceHealthStatus DefaultHealthy = new(true) { Message = "Healthy " }; 
        
        public string? Message { get; init; }
    }

    public enum ServiceStateStatus
    {
        Stopped,
        Running,
        Error
    }
}
