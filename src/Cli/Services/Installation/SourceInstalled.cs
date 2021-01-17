namespace Cli.Services.Installation
{
    internal record SourceInstalled(bool Installed, string Location, IServiceSource? Source) : ISourceInstalled
    {
        public static SourceInstalled At(IServiceSource source, string location) => new(true, location, source);

        public static string Key(IServiceSource source) => $"${source.Name}.Installed";

        public static SourceInstalled Nowhere() => new(false, string.Empty, null);
    }
}
