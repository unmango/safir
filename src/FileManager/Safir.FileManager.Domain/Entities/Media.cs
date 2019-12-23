using Safir.Common.Domain;
using Safir.FileManager.Domain.Events;
using Safir.FileManager.Domain.Models;

namespace Safir.FileManager.Domain.Entities
{
    public class Media : Entity<int>
    {
        public Media(Location location)
        {
            Location = location;
        }

        public Location Location { get; private set; }

        public void Move(Location newLocation)
        {
            var oldLocation = Location;

            Location = newLocation;

            Add(new MediaMoved(oldLocation, newLocation));
        }
    }
}
