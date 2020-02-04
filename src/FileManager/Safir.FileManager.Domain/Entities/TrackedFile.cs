using Safir.Common.Domain;

namespace Safir.FileManager.Domain.Entities
{
    public class TrackedFile : Entity<int>
    {
        public TrackedFile(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
