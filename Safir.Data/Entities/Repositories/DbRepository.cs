using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Safir.Data.Entities.Repositories
{
    public abstract class DbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private IAmbientDbContextLocator _contextLocator;

        public DbRepository(IAmbientDbContextLocator contextLocator) {
            _contextLocator = contextLocator
                ?? throw new ArgumentNullException(nameof(contextLocator));
        }

        internal MusicContext Context {
            get {
                return _contextLocator.Get<MusicContext>()
                    ?? throw new InvalidOperationException("No ambient DbContext of type MusicContext found");
            }
        }

        internal DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public virtual TEntity GetByID(object id) {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity) {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id) {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete) {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
                DbSet.Attach(entityToDelete);
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate) {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Save() {
            Context.SaveChanges();
        }
    }
}
