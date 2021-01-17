namespace Cli.Services.Installation
{
    internal interface ISourceInstalled
    {
        bool Installed { get; }
        
        string Location { get; }
        
        IServiceSource? Source { get; }
    }
}
