using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Enums;
using System;

namespace MovieUniverse.Abstract.Models
{
    public class UserConnectionModel
    {
        public long? Id { get; set; }
        public UserRelationship Type { get; set; }

        public UserConnectionModel(int? status,long? requestId)
        {
            if(status.HasValue)
            {
                Id = requestId;
                switch (status.Value)
                {
                    
                    case 0:
                        Type = UserRelationship.FriendRequestSent;
                        break;
                    case 1:
                        Type = UserRelationship.Friends;
                        Id = null;
                        break;
                    case 2:
                        Type = UserRelationship.FriendRequestReceived;
                        break;
                    case -1:
                        Type = UserRelationship.None;
                        Id = null;
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Type = UserRelationship.None;
            }

        }
        public UserConnectionModel(FriendRequestStatus? status,long? id,bool isRequestRecived)
        {
            Id = id;
            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case FriendRequestStatus.Accept:
                        Type = UserRelationship.Friends;
                        Id = null;
                        break;
                    case FriendRequestStatus.Reject:
                        Type = UserRelationship.None;
                        Id = null;
                        break;
                    case FriendRequestStatus.None:
                        Type = isRequestRecived
                            ? UserRelationship.FriendRequestReceived
                            : UserRelationship.FriendRequestSent;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                Type = UserRelationship.None;
            }
        }

        
    }
}