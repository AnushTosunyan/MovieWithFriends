using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities;

namespace MovieUniverse.Abstract.Data_Access_Contract
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<T> Repository<T>() where T : ModelBase;


        void SetAutoDetectChanges(bool isAuto);

        void BeginTransaction();

        void RollBack();

        void Save();

        void CommitTransaction();
    }
}
