using System;
using System.Collections.Generic;
using System.Linq;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;

namespace MovieUniverse.Abstract.Services
{
    public interface IService<T>:IDisposable where T:class,IEntity
    {
        T GetById(long id);

        IEnumerable<T> GetAll(FilterBase<T> filter=null);
        T Insert(T obj);
        T Update(T obj);
        T Delete(long id);

        int GetCount(FilterBase<T> filter = null);
    }
}