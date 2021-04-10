using Akka.Actor;

namespace Safir.Agent.Actors
{
    internal sealed class FileEventBusActor : ReceiveActor
    {
        public FileEventBusActor()
        {
            Receive<FileWatcherActor.Created>(OnFileCreated);
            Receive<FileWatcherActor.Changed>(OnFileChanged);
            Receive<FileWatcherActor.Deleted>(OnFileDeleted);
            Receive<FileWatcherActor.Renamed>(OnFileRenamed);
        }

        private void OnFileCreated(FileWatcherActor.Created message)
        {
            // TODO
        }

        private void OnFileChanged(FileWatcherActor.Changed message)
        {
            // TODO
        }

        private void OnFileDeleted(FileWatcherActor.Deleted message)
        {
            // TODO
        }

        private void OnFileRenamed(FileWatcherActor.Renamed message)
        {
            // TODO
        }
    }
}
