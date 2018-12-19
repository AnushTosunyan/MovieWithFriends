using System.Collections;
using System.Collections.Generic;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Data_Access_Contract.Repositories
{
    public interface IFriendRepository : IRepository<Friend>
    {
        bool IsFriends(long userId, long friendUserId);

        long GetAcceptedConnectionId(long userId,long friendUserId);


        Friend GetFriendRequest(long requestId);

        Friend GetFriendRequest(long userId, long friendUserId);

        IEnumerable<AppUser> GetFriends(long? userId,long requestedUserId,FilterBase<Friend> filter = null);

        IEnumerable<Friend> GetRecivedFriendRequests(long userId, FilterBase<Friend> filter = null);

        IEnumerable<Friend> GetSentFriendRequests(long userId, FilterBase<Friend> filter = null);
        
    }
}