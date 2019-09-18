using Safir.Common.Domain;

namespace Safir.FileManager.Domain.Entities
{
    public class Library : Entity
    {
        public Library(string physicalPath)
        {
            PhysicalPath = physicalPath;
        }
        
        public string PhysicalPath { get; }
    }
}
