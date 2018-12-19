using System;
using System.Collections.Generic;
using System.Linq;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Exceptions;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;
using MovieUniverse.Abstract.Services.UserServices;
using Ninject;

namespace MovieUniverse.Services.ServiceImpl.UserServicesImpl
{
    public class FriendService : ServiceBase<Friend>, IFriendService
    {
        private IFriendRepository Repository => GenereicRepository as IFriendRepository;

        [Inject]
        private IAppUserService UserService { get; set; }
        public long MakeFriendRequest(long userId, long friendUserId)
        {
            if(!UserService.IsUserActive(friendUserId))
                throw new MovieUniverseException(ExceptionType.UserByIdNotExist);

            if (userId == friendUserId)
                throw new MovieUniverseException(ExceptionType.YouCannotFriendYourself);

            var referseFriendship = Repository.GetFriendRequest(friendUserId, userId);
            if (referseFriendship != null && referseFriendship.Status != FriendRequestStatus.Reject)
            {
                var exception = new MovieUniverseException(ExceptionType.FriendRequestAlreadyRecived);
                throw exception;
            }
            
            var connection = Repository.GetFriendRequest(userId, friendUserId);
            if (connection != null)
            {
                if (connection.Status == FriendRequestStatus.Accept)
                    throw new MovieUniverseException(ExceptionType.AlreadyFriends);

                if (connection.Status == FriendRequestStatus.Reject)
                {
                    connection.Status = FriendRequestStatus.None;
                    connection.SentDate = DateTimeOffset.Now;
                    Repository.Save();
                    return connection.Id;
                }
                else
                {
                    throw new MovieUniverseException(ExceptionType.FriendRequestAlreadySent);
                }
            }
            else
            {
                Friend fr = new Friend()
                {
                    UserId = userId,
                    FriendUserId = friendUserId,
                    SentDate = DateTimeOffset.Now,
                    Status = FriendRequestStatus.None,
                };
                fr = Repository.Insert(fr);
                Repository.Save();
                return fr.Id;
            }
            
        }
        public void AcceptFriendRequest(long userId, long requestId)
        {
            var connection = Repository.GetFriendRequest(requestId);
            if (connection != null && connection.FriendUserId == userId)
            {
                if (connection.Status != FriendRequestStatus.None)
                    throw new MovieUniverseException(ExceptionType.NoSuitableRequestExsit);

                connection.Status = FriendRequestStatus.Accept;
                connection.AcceptedDate = DateTimeOffset.Now;
                var reverseFriendship = Repository.GetFriendRequest(connection.FriendUserId, connection.UserId);
                if (reverseFriendship == null)
                {
                    reverseFriendship = new Friend()
                    {
                        UserId = connection.FriendUserId,
                        FriendUserId = connection.UserId,
                        Status = FriendRequestStatus.Accept,
                        AcceptedDate = connection.AcceptedDate,
                    };
                    Repository.Insert(reverseFriendship);
                }
                else
                {
                    reverseFriendship.Status = FriendRequestStatus.Accept;
                    reverseFriendship.AcceptedDate = connection.AcceptedDate;
                }


                Repository.Save();
            }
            else
            {
                throw new MovieUniverseException(ExceptionType.FriendRequestNotExist);
            }
        }
        public void RejectFriendRequest(long userId, long requestId)
        {
            var connection = Repository.GetFriendRequest(requestId);
            if (connection != null && userId == connection.FriendUserId)
            {
                if (connection.Status == FriendRequestStatus.None)
                {
                    connection.Status = FriendRequestStatus.Reject;
                    Repository.Save();
                }
                else if (connection.Status == FriendRequestStatus.Reject)
                {
                    throw new MovieUniverseException(ExceptionType.FriendRequestAlreadyRejected);
                }
                else
                {
                    throw new MovieUniverseException(ExceptionType.AlreadyFriends);
                }
            }
            else
            {
                throw new MovieUniverseException(ExceptionType.FriendRequestNotExist);
            }
        }
        public void CancelFriendRequest(long userId, long requestId)
        {
            var connection = Repository.GetFriendRequest(requestId);
            if (connection != null && connection.UserId == userId && connection.Status == FriendRequestStatus.None)
            {
                Repository.Delete(requestId);
                Repository.Save();
                return;
            }
            throw new MovieUniverseException(ExceptionType.NoSuitableRequestExsit);
        }
        public void DeleteFriend(long userId, long friendUserId)
        {
            if (Repository.IsFriends(userId, friendUserId))
            {
                var connectionId = Repository.GetAcceptedConnectionId(userId, friendUserId);
                Repository.Delete(connectionId);
                var reverseConnectionId = Repository.GetAcceptedConnectionId(friendUserId, userId);
                Repository.Delete(reverseConnectionId);
                Repository.Save();
                return;
            }
            throw new MovieUniverseException(ExceptionType.YouAreNotFriends);
        }

        public IEnumerable<AppUser> GetFriendsList(long? userId,long requestedUserId, FilterBase<Friend> filter = null)
        {
            if(!UserService.IsUserActive(requestedUserId))
                throw new MovieUniverseException(ExceptionType.UserByIdNotExist);

            if (!UserService.CheckUserAccessStatus(userId.Value, requestedUserId))
                throw new MovieUniverseException(ExceptionType.YouHaveNoPermission);

            
            return Repository.GetFriends(userId, requestedUserId, filter);
        }
        
        public IEnumerable<Friend> GetRecivedFriendRequests(long userId, FilterBase<Friend> filter = null)
        {
            return Repository.GetRecivedFriendRequests(userId,filter);
        }

        public IEnumerable<Friend> GetSentFirendRequests(long userId, FilterBase<Friend> filter = null)
        {
            return Repository.GetSentFriendRequests(userId,filter);
        }
        
    }
}