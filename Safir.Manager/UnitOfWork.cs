using System;
using System.Collections.Generic;
using System.Linq;

namespace Safir.Manager
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HashSet<IRepository> _repositories;

        public void Register(IRepository repo)
        {
            _repositories.Add(repo);
        }

        public void Commit()
        {
            //// Since repositores share a data context, we can just call
            //// save on the context, rather than iterating through repos
            //// Plus better performance
            // JK not all repositories will be EF repos
            //_context.SaveChanges();
            _repositories.ToList().ForEach(x => x.Save());
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
