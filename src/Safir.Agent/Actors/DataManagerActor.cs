using Akka.Actor;
using JetBrains.Annotations;

namespace Safir.Agent.Actors
{
    [UsedImplicitly]
    internal sealed class DataManagerActor : ReceiveActor
    {
        private readonly IActorRef _optionsMonitor;
        private IActorRef? _directory;
        private IActorRef? _watcher;

        public DataManagerActor(IActorRef optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;

            Receive<DataDirectoryChange>(OnDataDirectoryChange);
        }

        protected override void PreStart()
        {
            _directory = Context.ActorOf(Props.Create<DataDirectoryActor>(_optionsMonitor), "directory");
            _directory.Tell(new GetDataDirectory());
        }

        private void OnDataDirectoryChange(DataDirectoryChange message)
        {
            _watcher = Context.ActorOf(Props.Create(() => new FileWatcherActor(Self, message.Path)));
        }
    }
}
