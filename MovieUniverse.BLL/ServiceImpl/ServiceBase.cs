using System;
using System.Collections.Generic;
using System.Linq;
using MovieUniverse.Abstract;
using MovieUniverse.Abstract.Data_Access_Contract;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Services;
using MovieUniverse.DAL.DataAccess;
using Ninject;

namespace MovieUniverse.Services.ServiceImpl
{
    public class ServiceBase<T> : IService<T> where T : class,IEntity
    {
        [Inject]
        protected IRepository<T> GenereicRepository { get; set; }

        public virtual IEnumerable<T> GetAll(FilterBase<T> filter=null)
        {
            return GenereicRepository.Query().Filter(filter).Get();
        }

        public virtual T GetById(long id)
        {
            return GenereicRepository.Query().Where(x => x.Id == id).Get().FirstOrDefault();
        }

        public virtual T Insert(T obj)
        {
            T newObj = GenereicRepository.Insert(obj);
            GenereicRepository.Save();
            return newObj;
        }

        public virtual T Update(T obj)
        {
            //T newobj = UnitOfWork.Repository<T>().Update(obj);
            //UnitOfWork.Save();
            //return newobj;
            return null;
        }
        public virtual T Delete(long id)
        {
            T newObj = GenereicRepository.Delete(id);
            GenereicRepository.Save();
            return newObj;
        }
  
        public void Dispose()
        {
            GenereicRepository.Dispose();
        }

        public virtual int GetCount(FilterBase<T> filter = null)
        {
            if (filter != null)
            {
                filter.SelectCount = null;
                filter.SkipCount = null;
            }
            return GenereicRepository.GetCount(filter);
        }
    }
}
