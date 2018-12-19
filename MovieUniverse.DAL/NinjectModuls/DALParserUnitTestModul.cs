using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.DB;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.DAL.DataAccess.Repositories;
using MovieUniverse.DAL.DataAccess.Repositories.UserRepositories;
using MovieUniverse.DB.DbContext;
using Ninject.Modules;
using Ninject.Web.Common;

namespace MovieUniverse.DAL.NinjectModuls
{
    public class DALParserUnitTestModul : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Movie>>().To<MovieRepository>().InRequestScope();
            Bind<IRepository<Genre>>().To<GenreRepository>().InRequestScope();
            Bind<IRepository<AppUser>>().To<AppUserRepository>().InRequestScope();


            Bind<IRepository<Friend>>().To<FriendRepository>().InRequestScope();
            Bind<IRepository<UserWatchMovie>>().To<UserWatchMovieRepository>().InRequestScope();
            Bind<IRepository<Rating>>().To<RatingRepository>().InRequestScope();


            Bind<IDbContext>().To<MovieUniverseContext>().InSingletonScope();

            Bind<IMovieRepository>().To<MovieRepository>().InRequestScope();
            Bind<IGenreRepository>().To<GenreRepository>().InRequestScope();
            Bind<IRegionRepository>().To<RegionRepository>().InRequestScope();
            Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            Bind<IPersonRepository>().To<PersonRepository>().InRequestScope();


            Bind<IAppUserRepository>().To<AppUserRepository>().InRequestScope();





            Bind<IFriendRepository>().To<FriendRepository>().InRequestScope();
            Bind<IUserWatchMovieRepository>().To<UserWatchMovieRepository>().InRequestScope();
        }
    }
}