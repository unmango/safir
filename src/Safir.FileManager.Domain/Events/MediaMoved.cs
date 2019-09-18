using Safir.Common.Domain;
using Safir.FileManager.Domain.Models;

namespace Safir.FileManager.Domain.Events
{
    public class MediaMoved : DomainEvent
    {
        public MediaMoved(Location oldLocation, Location newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }

        public Location OldLocation { get; }

        public Location NewLocation { get; }
    }
}
