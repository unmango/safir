// <copyright file="FileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TagLib;

    public abstract class FileRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private IEnumerable<File> _files;

        public FileRepository(IEnumerable<File> files)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public virtual TEntity GetByID(object id)
        {
            if (id is File)
                id = id as File;
            _files.ToList().Find(x => x == id);

            throw new NotImplementedException();
        }

        public virtual void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Update(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }
    }
}
