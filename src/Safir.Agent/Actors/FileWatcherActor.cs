using System.IO;
using Akka.Actor;
using Akka.Event;

namespace Safir.Agent.Actors
{
    internal sealed class FileWatcherActor : ReceiveActor
    {
        public abstract record FileSystemMessage(string? Name, string Root, string Path);
        
        public record Created(string? Name, string Root, string Path) : FileSystemMessage(Name, Root, Path);

        public record Changed(string? Name, string Root, string Path) : FileSystemMessage(Name, Root, Path);

        public record Renamed(string? Name, string Root, string Path) : FileSystemMessage(Name, Root, Path);

        public record Deleted(string? Name, string Root, string Path) : FileSystemMessage(Name, Root, Path);
        
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _subscriber;
        private readonly string _path;
        private FileSystemWatcher? _watcher;

        public FileWatcherActor(IActorRef subscriber, string path)
        {
            _subscriber = subscriber;
            _path = path;
        }

        protected override void PreStart()
        {
            _watcher = new FileSystemWatcher(_path) {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };

            _watcher.Created += OnCreated;
            _watcher.Changed += OnChanged;
            _watcher.Renamed += OnRenamed;
            _watcher.Deleted += OnDeleted;
            _watcher.Error += OnError;
        }

        protected override void PostStop()
        {
            if (_watcher == null) return;

            _watcher.Created -= OnCreated;
            _watcher.Changed -= OnChanged;
            _watcher.Renamed -= OnRenamed;
            _watcher.Deleted -= OnDeleted;
            _watcher.Error -= OnError;
            
            _watcher.Dispose();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.Debug("File created: {FullPath}", e.FullPath);
            _subscriber.Tell(new Created(e.Name, _path, e.FullPath));
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.Debug("File changed: {FullPath}", e.FullPath);
            _subscriber.Tell(new Changed(e.Name, _path, e.FullPath));
        }

        private void OnRenamed(object sender, FileSystemEventArgs e)
        {
            _logger.Debug("File renamed: {FullPath}", e.FullPath);
            _subscriber.Tell(new Renamed(e.Name, _path, e.FullPath));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.Debug("File deleted: {FullPath}", e.FullPath);
            _subscriber.Tell(new Deleted(e.Name, _path, e.FullPath));
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.Debug(e.GetException(), "An error occurred");
        }
    }
}
