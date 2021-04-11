using MediatR;

namespace Safir.Agent.Events
{
    public interface IFileSystemEvent : INotification { }
    
    public record FileChanged(string Path) : IFileSystemEvent;
    
    internal record FileCreated(string Path) : IFileSystemEvent;
    
    public record FileDeleted(string Path) : IFileSystemEvent;
    
    public record FileRenamed(string Path, string OldPath) : IFileSystemEvent;
}
