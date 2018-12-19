using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;

namespace MovieUniverse.Abstract.Data_Access_Contract.Repositories
{
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        T Insert(T entity);
        T Delete(long id);
        T Update(T entity);
        int GetCount(FilterBase<T> filter = null);

        IEnumerable<T> GetAll(FilterBase<T> filter = null);

        IEnumerable<T> Find(Expression<Func<T, bool>> predictate);
        T GetById(long id);
        void Attach(T entity);
        void DeAttach(T entity);
        void Save();
        IRepositoryQuery<T> Query();
    }
}
