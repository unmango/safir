using Akka.Actor;
using Akka.Event;

namespace Safir.Agent.Actors
{
    internal sealed class FileEventBusActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _eventBus;

        public FileEventBusActor(IActorRef eventBus)
        {
            _eventBus = eventBus;
            
            Receive<FileWatcherActor.Created>(OnFileCreated);
            Receive<FileWatcherActor.Changed>(OnFileChanged);
            Receive<FileWatcherActor.Deleted>(OnFileDeleted);
            Receive<FileWatcherActor.Renamed>(OnFileRenamed);
        }

        private void OnFileCreated(FileWatcherActor.Created message)
        {
            _logger.Debug("Publishing created event to bus");
            _eventBus.Tell(message);
        }

        private void OnFileChanged(FileWatcherActor.Changed message)
        {
            _logger.Debug("Publishing changed event to bus");
            _eventBus.Tell(message);
        }

        private void OnFileDeleted(FileWatcherActor.Deleted message)
        {
            _logger.Debug("Publishing deleted event to bus");
            _eventBus.Tell(message);
        }

        private void OnFileRenamed(FileWatcherActor.Renamed message)
        {
            _logger.Debug("Publishing renamed event to bus");
            _eventBus.Tell(message);
        }
    }
}
