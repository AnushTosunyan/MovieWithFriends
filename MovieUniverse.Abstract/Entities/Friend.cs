using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace MovieUniverse.Abstract.Entities.UserEntityes
{
    public class Friend : ModelBase
    {
        [ForeignKey("User"), Index("UserIdFriendUserId", 1, IsUnique = true)]
        public long UserId { get; set; }

        [ForeignKey("FriendUser"), Index("UserIdFriendUserId", 2, IsUnique = true)]
        public long FriendUserId { get; set; }
        [Range(-1, 1)]
        public DateTimeOffset? SentDate { get; set; }
        public DateTimeOffset? AcceptedDate { get; set; }
        public AppUser User { get; set; }
        public FriendRequestStatus Status { get; set; }
  
        public AppUser FriendUser { get; set; }
    }
}