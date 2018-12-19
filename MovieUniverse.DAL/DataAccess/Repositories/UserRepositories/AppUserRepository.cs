using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Exceptions;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.DAL.DataAccess.Repositories.UserRepositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {

        public bool IsUserActive(long userId)
        {
            bool b = DbSet.Where(x => x.Id == userId).Select(x => x.EmailComfirmed).ToList().FirstOrDefault();
            return b;

            //return user.EmailComfirmed;
        }
        public bool IsPublicUser(long userId)
        {
            var isPublic = DbSet.Find(userId);
            if (isPublic != null)
                return isPublic.IsPublicUser;
            throw new MovieUniverseException(ExceptionType.UserByIdNotExist);
        }


        public AppUser GetUser(long userId)
        {
            return DbSet.SingleOrDefault(x => x.Id == userId);
        }
        public AppUser GetUser(string email)
        {
            var user = DbSet.Where(x => x.Email == email).Include(x => x.Token).FirstOrDefault();
            return user;
        }
        public AppUser GetOAuthUser(OAuthUserLoginModel model)
        {
            var user =
                DbSet.Where(x => x.Provider == model.Provider && x.ProviderUserId == model.ProviderId).Include(x => x.Token).FirstOrDefault();
            return user;

        }
        public override AppUser Delete(long id)
        {
            Token entity = Context.Set<Token>().Find(id);
            Context.Entry(entity).State = EntityState.Deleted;
            return base.Delete(id);
        }

        public string GetUserPassword(string email)
        {
            return DbSet.Where(x => x.Email == email).Select(x => x.PasswordHash).FirstOrDefault();
        }


        public Token GetToken(string key)
        {
            return Context.Set<Token>().SingleOrDefault(x => x.Key == key);
        }
        public Token GetToken(long userId)
        {
            return Context.Set<Token>().FirstOrDefault(x => x.UserId == userId);
        }

        public Token InsertToken(Token token)
        {
            token = Context.Set<Token>().Add(token);
            Context.Entry(token).State = EntityState.Added;
            return token;
        }
        //TODO disabled user activity can be shown
        public IEnumerable<UserUserActivity> GetUserUserActivities(long userId)
        {
            var link = from friend1 in Context.Set<Friend>()
                       join friend2 in Context.Set<Friend>() on friend1.FriendUserId equals friend2.UserId
                       where
                           friend1.UserId == userId && friend2.FriendUserId != userId &&
                           friend1.Status == FriendRequestStatus.Accept && friend2.Status == FriendRequestStatus.Accept
                       select
                           new
                           {
                               FriendUserId = friend1.FriendUserId,
                               BecomeFriendUserId = friend2.FriendUserId,
                               Date = friend1.AcceptedDate,
                           };

            var link1 = from user1 in DbSet
                        join activity in link on user1.Id equals activity.FriendUserId
                        join user2 in DbSet on activity.BecomeFriendUserId equals user2.Id
                        select new
                        {
                            Command = "Friends",
                            Date = activity.Date,
                            Owner = new
                            {
                                Id = user1.Id,
                                Name = user1.Name,
                                PhotoUrl = user1.PhotoUrl,
                            },
                            Subject = new
                            {
                                Id = user2.Id,
                                Name = user2.Name,
                                PhotoUrl = user2.PhotoUrl,
                            },
                        };

            var lst = link1.ToList().Select(x => new UserUserActivity()
            {
                Command = x.Command,
                Date = x.Date.Value,
                Owner = new AppUser()
                {
                    Id = x.Owner.Id,
                    Name = x.Owner.Name,
                    PhotoUrl = x.Owner.PhotoUrl
                },
                Subject = new AppUser()
                {
                    Id = x.Subject.Id,
                    Name = x.Subject.Name,
                    PhotoUrl = x.Subject.PhotoUrl,
                }
            });
            return lst;
        }

        public IEnumerable<UserMovieActivity> GetUserMovieActivities(long userId) {
            throw new NotImplementedException() { };
        }

        public IEnumerable<UserMovieActivity> GetUserRateMoiveActivites(long userId)
        {
            throw new NotImplementedException()
            {
            };
        }     
        public bool IsFriends(long userId, long friendUserId)
        {
            return
                Context.Set<Friend>().Any(
                    x => x.UserId == userId && x.FriendUserId == friendUserId && x.Status == FriendRequestStatus.Accept);
        }

        public AppUser GetById(long userId, long requestedUserId)
        {
            //StopWatch sw = new StopWatch();
            //sw.Start();
            bool isRequestRecived = false;
            var user = GetUser(requestedUserId);
            var connection =
                Context.Set<Friend>().FirstOrDefault(x => x.UserId == userId && x.FriendUserId == requestedUserId);
            if (connection == null)
            {
                connection = Context.Set<Friend>().FirstOrDefault(x => x.UserId == requestedUserId && x.FriendUserId == userId);
                isRequestRecived = true;
            }
            if (userId != requestedUserId)
            {
                FriendRequestStatus? status = connection?.Status;
                user.Connection = new UserConnectionModel(status, connection?.Id, isRequestRecived);
            }

            user.FriendsCount = Context.Set<Friend>().Count(x => x.UserId == requestedUserId && x.Status == FriendRequestStatus.Accept);
            return user;

        }

        private IQueryable<long> GetFriendListIds(long userId)
        {
            var link = from friend in Context.Set<Friend>()
                       where friend.UserId == userId && friend.Status == FriendRequestStatus.Accept
                       join user in Context.Set<AppUser>() on friend.FriendUserId equals user.Id
                       where user.EmailComfirmed
                       select friend.FriendUserId;
            return link;
        }

        public IEnumerable<AppUser> GetAll(long? userId, FilterBase<AppUser> filter)
        {
            throw new NotImplementedException();
        }
    }
}