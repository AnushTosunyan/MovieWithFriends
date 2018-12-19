using System.Collections.Generic;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Data_Access_Contract.Repositories
{
    public interface IAppUserRepository:IRepository<AppUser>
    {


        AppUser GetOAuthUser(OAuthUserLoginModel model);
        bool IsPublicUser(long userId);
        bool IsFriends(long userId, long friendUserId);
        AppUser GetUser(long userId);
        AppUser GetUser(string email);

        AppUser GetById(long userId, long requestedUserId);
        IEnumerable<AppUser> GetAll(long? userId, FilterBase<AppUser> filter);
        string GetUserPassword(string email);

      

        Token GetToken(string key);
        Token GetToken(long userId);

        Token InsertToken(Token token);

        

        bool IsUserActive(long userId);

        IEnumerable<UserUserActivity> GetUserUserActivities(long userId);

        IEnumerable<UserMovieActivity> GetUserMovieActivities(long userId);

        IEnumerable<UserMovieActivity> GetUserRateMoiveActivites(long userId);
    }
}