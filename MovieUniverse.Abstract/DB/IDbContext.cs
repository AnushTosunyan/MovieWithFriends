using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.Abstract.DB
{
    public interface IDbContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        DbEntityEntry Entry(object o);
        void Dispose();
    }
}