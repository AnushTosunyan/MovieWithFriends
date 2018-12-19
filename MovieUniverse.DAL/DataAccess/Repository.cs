using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MovieUniverse.Abstract;
using MovieUniverse.Abstract.Data;
using MovieUniverse.Abstract.Data_Access_Contract;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.DB;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.DB.DbContext;
using Ninject;

namespace MovieUniverse.DAL.DataAccess
{
    public class Repository<T> : IInitializable,IRepository<T> where T : class,IEntity
    {
        [Inject]
        protected IDbContext Context { get; set; }

        protected IDbSet<T> DbSet;


        public T Insert(T entity)
        {
            DbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Added;
            return entity;
        }

        public virtual T Delete(long id)
        {
            T entity = DbSet.Find(id);
            Context.Entry(entity).State = EntityState.Deleted;
            return entity;
        }

        public T Update(T entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Attach(T entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void DeAttach(T entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
        public void Save()
        {
            Context.SaveChanges();
        }

        public virtual IRepositoryQuery<T> Query()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<T>(this);

            return repositoryGetFluentHelper;
        }


        internal IQueryable<T> Get(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,List<Expression<Func<T, object>>> includeProperties = null,FilterBase<T> filter = null)
        {
            IQueryable<T> query = DbSet;
            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (where != null)
                query = query.Where(where);
            if (orderBy != null)
                query = orderBy(query);
            if (filter != null)
                query = filter.Filter(query);
            return query;
        }

        public virtual void Initialize()
        {
            DbSet = Context.Set<T>();
            Debug.WriteLine("created" + Context.GetHashCode());
        }

        public void Dispose()
        {
            Debug.WriteLine("disposed" +Context.GetHashCode());
            Context.Dispose();
        }

        public int GetCount(FilterBase<T> filter = null)
        {
            if(filter != null)
                return filter.Filter(DbSet).Count();

            return DbSet.Count();
        }

        public virtual T GetById(long id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll(FilterBase<T> filter = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
                query = filter.Filter(query);
            return query.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predictate)
        {
            IQueryable<T> query = DbSet;
            return query.Where(predictate);
        }
    }
}
