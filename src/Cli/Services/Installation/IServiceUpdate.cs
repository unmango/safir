namespace Cli.Services.Installation
{
    internal interface IServiceUpdate
    {
        string CurrentVersion { get; }
        
        string UpdateVersion { get; }
        
        string? ChangeLog { get; }
        
        string? ReleaseNotes { get; }
    }
}
