using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Enums;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.DAL.DataAccess.Repositories.UserRepositories
{
    public class FriendRepository : Repository<Friend>, IFriendRepository
    {
        public long GetAcceptedConnectionId(long userId, long friendUserId)
        {
            var connection = GetFriendRequest(userId,friendUserId);
            if (connection != null && connection.Status == FriendRequestStatus.Accept)
                return connection.Id;
            return -1;
        }

        public Friend GetFriendRequest(long requestId)
        {
            var connection = DbSet.Find(requestId);
            return connection;
        }

        public Friend GetFriendRequest(long userId, long friendUserId)
        {
            return DbSet.FirstOrDefault(x => x.UserId == userId && x.FriendUserId == friendUserId);
        }
        
        public IEnumerable<AppUser> GetFriends(long? userId,long requestedUserId, FilterBase<Friend> filter = null)
        {
            var acceptedFriendRequests = DbSet.Where(x => x.Status == FriendRequestStatus.Accept && x.UserId == requestedUserId);
            var list = filter == null ? acceptedFriendRequests : filter.Filter(acceptedFriendRequests);

            //var friendsCount = from f in DbSet
            //    group f by new {f.UserId, f.Status}
            //    into newDb
            //    where newDb.Key.Status == FriendRequestStatus.Accept
            //    select new {newDb.Key.UserId, Count = newDb.Count()};

            #region movies count
           

            var userMoviesCount = Context.GetUserMovieFriendCounts();
            #endregion moviesCount

            var friends = from u1 in list
            join u2 in Context.Set<AppUser>() on u1.FriendUserId equals u2.Id
            where u2.EmailComfirmed
            join movieCount in userMoviesCount on u1.FriendUserId equals movieCount.UserId into movieDb
            from mCount in movieDb.DefaultIfEmpty()
            select new
            {
                User = new
                {
                    Id = u2.Id,
                    Name = u2.Name,
                    PhotoUrl = u2.PhotoUrl,
                    IsPublic = u2.IsPublicUser,
                    FriendsCount = (long?)mCount.FriendsCount ?? 0,
                    WatchMovieCount = (long?)mCount.WatchMoviesCount??0,
                    WatchedMovieCount = (long?)mCount.WatchedMoviesCount ?? 0
                }

            };

            if (userId.HasValue && userId.Value != -1)
            {
                var requestSetOrFriends = from u1 in Context.Set<Friend>()
                                          where u1.UserId == userId.Value
                                          select new { RequestId = u1.Id, UserId = u1.FriendUserId, Status = (int)u1.Status };
                var requestRecived = from u1 in Context.Set<Friend>()
                                     where u1.FriendUserId == userId.Value && u1.Status == FriendRequestStatus.None
                                     select new { RequestId = u1.Id, UserId = u1.UserId, Status = 2 };
                var relationShip = requestSetOrFriends.Union(requestRecived);



                var signedResult = from f in friends
                    join r in relationShip on f.User.Id equals r.UserId into newDb
                    from b in newDb.DefaultIfEmpty()
                    select
                        new
                        {
                            User = f.User,
                            RequestId = (long?) b.RequestId,
                            Status = (int?) b.Status
                        };

                return signedResult.ToList().Select(x => new AppUser()
                {
                    Id = x.User.Id,
                    Name = x.User.Name,
                    PhotoUrl = x.User.PhotoUrl,
                    IsPublicUser = x.User.IsPublic,
                    Connection = new UserConnectionModel(x.Status,x.RequestId),
                    FriendsCount = x.User.FriendsCount,
                    WatchMoviesCount = x.User.WatchMovieCount,
                    WatchedMoviesCount = x.User.WatchedMovieCount,
                });
            }
            
            return friends.ToList().Select(x => new AppUser()
            {
                Id = x.User.Id,
                Name = x.User.Name,
                PhotoUrl = x.User.PhotoUrl,
                IsPublicUser = x.User.IsPublic,
                FriendsCount = x.User.FriendsCount,
                WatchMoviesCount = x.User.WatchMovieCount,
                WatchedMoviesCount = x.User.WatchedMovieCount,
            });
        }

        public IEnumerable<Friend> GetRecivedFriendRequests(long userId, FilterBase<Friend> filter = null)
        {
            var list = DbSet.Where(x => x.FriendUserId == userId && x.Status == FriendRequestStatus.None);
            if (filter != null)
                list = filter.Filter(list);
            return list.Include(x => x.User).Where(x=>x.User.EmailComfirmed)
                .Select(x => new
                {
                    Id = x.Id,
                    User =  new 
                    {
                        Id = x.UserId,
                        Name = x.User.Name,
                        PhotoUrl = x.User.PhotoUrl,
                    }
                    
                }).ToList().Select(x => new Friend()
                {
                    Id = x.Id,
                    User = new AppUser()
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        PhotoUrl = x.User.PhotoUrl,
                    }

                }).ToList();
        }

        public IEnumerable<Friend> GetSentFriendRequests(long userId, FilterBase<Friend> filter = null)
        {

            var list = DbSet.Where(x => x.UserId == userId && x.Status == FriendRequestStatus.None);
            if (filter != null)
                list = filter.Filter(list);

            return list.Include(x => x.FriendUser).Where(x=>x.FriendUser.EmailComfirmed)
                .Select(x => new 
                {
                    Id = x.Id,
                    User = new
                    {
                        Id = x.FriendUserId,
                        Name = x.FriendUser.Name,
                        PhotoUrl = x.FriendUser.PhotoUrl,
                    }

                }).ToList().Select(x=> new Friend()
                {
                    Id = x.Id,
                    User = new AppUser()
                    {
                        Id = x.User.Id,
                        Name = x.User.Name,
                        PhotoUrl = x.User.PhotoUrl,
                    }

                }).ToList();
        }

        public bool IsFriends(long userId, long friendUserId)
        {
            return
                DbSet.Any(
                    x => x.UserId == userId && x.FriendUserId == friendUserId && x.Status == FriendRequestStatus.Accept);
        }



    }
}