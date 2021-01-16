namespace Cli.Services.Installation
{
    internal record ServiceInstalled(bool Installed, string Location) : IServiceInstalled
    {
        public static ServiceInstalled At(string location) => new(true, location);

        public static ServiceInstalled Nowhere() => new(false, string.Empty);
    }
}
