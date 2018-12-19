using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MovieUniverse.Abstract.Data_Access_Contract;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;

namespace MovieUniverse.DAL.DataAccess
{
    public sealed class RepositoryQuery<T>:IRepositoryQuery<T> where T:class,IEntity
    {
        private readonly List<Expression<Func<T, object>>> _includeProperties;

        private readonly Repository<T> _repository;
        private Expression<Func<T, bool>> _where;
        private FilterBase<T> _filter; 
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;

        public RepositoryQuery(Repository<T> repository)
        {
            _repository = repository;
            _includeProperties =
                new List<Expression<Func<T, object>>>();
        }

        public IRepositoryQuery<T> Where(Expression<Func<T, bool>> where)
        {
            _where = where;
            return this;
        }

        public IRepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public IRepositoryQuery<T> Include(Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }



        public IEnumerable<T> Get()
        {
            return _repository.Get(
                _where,
                _orderByQuerable, _includeProperties,_filter);
        }

        public IRepositoryQuery<T> Filter(FilterBase<T> filter)
        {
            _filter = filter;
            return this;
        }
    }
}