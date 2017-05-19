using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public interface IUnitOfWork : IDisposable
    {
        void Register(IRepository repo);
        void Save();
    }
}
