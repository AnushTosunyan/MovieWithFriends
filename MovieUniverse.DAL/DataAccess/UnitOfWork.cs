using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieUniverse.Abstract;
using MovieUniverse.Abstract.Data_Access_Contract;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.DAL.DataAccess.Repositories;
using MovieUniverse.DB.DbContext;

namespace MovieUniverse.DAL.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieUniverseContext _db = new MovieUniverseContext();

        private readonly Dictionary<Type, object> _repos = new Dictionary<Type, object>();

        private MovieRepository _movieRepository;

        //public IMovieRepository MovieRepository => _movieRepository ?? (_movieRepository = new MovieRepository(_db));


        //private GenreRepository _genreRepository;

        //public IGenreRepository GenreRepository => _genreRepository ?? (_genreRepository = new GenreRepository(_db));


        //private PersonRepository _personRepository;

        //public PersonRepository PersonRepository => _personRepository ?? (_personRepository = new PersonRepository(_db));

        public IRepository<T> Repository<T>() where T : ModelBase
        {

            //Type type = typeof(T);
            //if (_repos.ContainsKey(type))
            //    return _repos[type] as IRepository<T>;

            //IRepository<T> repo = new Repository<T>(_db);
            //_repos.Add(type, repo);
            //return repo;
            return null;
        }

        public void SetAutoDetectChanges(bool isAuto)
        {
            _db.Configuration.AutoDetectChangesEnabled = isAuto;
        }

        public void BeginTransaction()
        {
            _db.Database.BeginTransaction();
        }

        public void RollBack()
        {
            _db.Database.CurrentTransaction.Rollback();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void CommitTransaction()
        {
            _db.Database.CurrentTransaction.Commit();
        }

        public void Dispose()
        {
            //_db.Database.CurrentTransaction.Dispose();
            _db.Dispose();
        }
    }
}
