using System;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Entities.UserEntityes.Room;

namespace MovieUniverse.Abstract.Models.Notifications
{
    public class RoomInvitationNotification:Notification
    {
        public Room Room { get; set; }
        public AppUser Inviter { get; set; }
        public DateTimeOffset Date { get; set; }
        public long RequestId { get; set; }
    }
}