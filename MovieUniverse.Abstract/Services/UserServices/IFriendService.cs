using System.Collections.Generic;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Services.UserServices
{
    public interface IFriendService:IService<Friend>
    {
        long MakeFriendRequest(long userId, long friendUserId);

        void DeleteFriend(long userId, long friendUserId);
        

        void AcceptFriendRequest(long userId, long requestId);

        void RejectFriendRequest(long userId, long requestId);

        void CancelFriendRequest(long userId, long requestId);

        IEnumerable<AppUser> GetFriendsList(long? userId, long requestedUserId, FilterBase<Friend> filter = null);

        IEnumerable<Friend> GetRecivedFriendRequests(long userId, FilterBase<Friend> filter = null);

        IEnumerable<Friend> GetSentFirendRequests(long userId, FilterBase<Friend> filter = null);
    }
}