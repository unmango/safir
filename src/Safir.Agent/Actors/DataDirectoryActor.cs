using Akka.Actor;
using JetBrains.Annotations;

namespace Safir.Agent.Actors
{
    [UsedImplicitly]
    internal sealed class DataDirectoryActor : ReceiveActor
    {
        private readonly IActorRef _optionsMonitor;
        private string? _path;

        public DataDirectoryActor(IActorRef optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;

            Receive<GetDataDirectory>(OnGetDataDirectory);
            Receive<OptionsValue>(OnOptionsValue);
            Receive<OptionsChange>(OnOptionsChange);
        }

        protected override void PreStart()
        {
            _optionsMonitor.Tell(new GetOptionsValue());
            _optionsMonitor.Tell(new Subscribe(Self));
        }

        protected override void PostStop()
        {
            _optionsMonitor.Tell(new Unsubscribe(Self));
        }

        private void OnGetDataDirectory(GetDataDirectory message)
        {
            if (string.IsNullOrWhiteSpace(_path)) return;
            
            Sender.Tell(new DataDirectoryChange(_path, null));
        }

        private void OnOptionsValue(OptionsValue message)
        {
            _path = message.Options.DataDirectory;
        }

        private void OnOptionsChange(OptionsChange message)
        {
            // TODO: Validation
            var oldPath = _path;
            _path = message.Options.DataDirectory ?? string.Empty;
            Context.Parent.Tell(new DataDirectoryChange(_path, oldPath));
        }
    }
}
