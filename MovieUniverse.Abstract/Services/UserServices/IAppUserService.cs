using System.Collections.Generic;
using System.Threading.Tasks;
using MovieUniverse.Abstract.ApiModels;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Services.UserServices
{
    public interface IAppUserService:IService<AppUser>
    {
        bool IsUserPublic(long userId);
        bool IsFriends(long userId, long friendUserId);
        bool CheckUserAccessStatus(long userId, long requestedUserId);

        IEnumerable<AppUser> GetAll(long? userId, FilterBase<AppUser> filter);
        AppUser GetById(long userId, long requestedUserId);
        
        AppUser EditUser(long userId, UserEditModel user);
        AppUser Login(UserLoginModel loginModel);

        long GetUserIdByToken(string token);

        AppUser OAuthLogin(OAuthUserLoginModel loginModel);

        void RegisterUser(AppUser user);

        void ConfirmUser(AccountActivationModel model);

        bool IsUserActive(long userId);

        IEnumerable<UserUserActivity> GetUserUserActivities(long userId);

        IEnumerable<UserMovieActivity> GetUserMovieActivities(long userId);

        IEnumerable<UserMovieActivity> GetUserRateMovieActivities(long userId);

    }
}