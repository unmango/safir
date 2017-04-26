using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    class SongRepository
    {
        private readonly Hierarchy _hierarchy;

        public SongRepository(Hierarchy hierarchy)
        {
            _hierarchy = hierarchy;
        }
    }
}
