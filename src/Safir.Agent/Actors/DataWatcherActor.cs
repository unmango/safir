using Akka.Actor;
using Microsoft.Extensions.FileProviders;
using Safir.Agent.Services;

namespace Safir.Agent.Actors
{
    public record StartWatching(string Path);
    
    internal sealed class DataWatcherActor : ReceiveActor
    {
        private readonly IFileProviderFactory _fileProviderFactory;
        private IFileProvider? _root;
        
        public DataWatcherActor(IFileProviderFactory fileProviderFactory)
        {
            _fileProviderFactory = fileProviderFactory;

            Receive<StartWatching>(StartWatching);
        }

        private void StartWatching(StartWatching message)
        {
            _root = _fileProviderFactory.Create(message.Path);
        }
    }
}
