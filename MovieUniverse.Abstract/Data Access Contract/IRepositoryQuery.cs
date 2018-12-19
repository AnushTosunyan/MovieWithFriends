using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;

namespace MovieUniverse.Abstract.Data_Access_Contract
{
    public interface IRepositoryQuery<T> where T:IEntity
    {
        IRepositoryQuery<T> Where(Expression<Func<T, bool>> filter);

        IRepositoryQuery<T> Filter(FilterBase<T> filer);
        IRepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        IRepositoryQuery<T> Include(Expression<Func<T, object>> expression);

        IEnumerable<T> Get();
    }
}