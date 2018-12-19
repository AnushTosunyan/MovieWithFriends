using MovieUniverse.Abstract.Services;
using MovieUniverse.Abstract.Services.UserServices;
using MovieUniverse.Services.ServiceImpl;
using MovieUniverse.Services.ServiceImpl.UserServicesImpl;
using Ninject.Modules;
using Ninject.Web.Common;

namespace MovieUniverse.Services.NinjectModules
{
    public class ServiceModule:NinjectModule
    {
        public override void Load()
        {
            
            Bind<IMovieService>().To<MovieService>().InRequestScope();
            Bind<IGenreService>().To<GenreService>().InRequestScope();
            Bind<IAppUserService>().To<AppUserService>().InRequestScope();
            Bind<IFriendService>().To<FriendService>().InRequestScope();

            Bind<IUserWatchMovieService>().To<UserWatchMovieService>().InRequestScope();
            Bind<IRatingService>().To<RatingService>().InRequestScope();
            Bind<IEmailService>().To<EmailService>().InRequestScope();

            Bind<IRoomService>().To<RoomService>().InRequestScope();



        }
    }
}