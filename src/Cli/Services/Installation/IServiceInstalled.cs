namespace Cli.Services.Installation
{
    internal interface IServiceInstalled
    {
        bool Installed { get; }
        
        string Location { get; }
    }
}
