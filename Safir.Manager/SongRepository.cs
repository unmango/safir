using Safir.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public class SongRepository : Repository<Song>
    {
        public SongRepository(IDbContext context, IUnitOfWork unitOfWork)
            : base(context, unitOfWork) { }
    }
}
