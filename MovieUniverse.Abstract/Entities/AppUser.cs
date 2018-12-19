using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Entities.UserEntityes
{
    public class AppUser:ModelBase,IEntity
    {
        [Index("EmailProviderProviderId",1,IsUnique = true),MaxLength(40)]
        public string Email { get; set; }
        [Required,MaxLength(60)]
        public string Name { get; set; }
        public bool EmailComfirmed { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        [Index("EmailProviderProviderId", 2, IsUnique = true), MaxLength(15)]
        public string Provider { get; set; }
        [Index("EmailProviderProviderId", 3, IsUnique = true), MaxLength(25)]
        public string ProviderUserId { get; set; }
        public bool IsPublicUser { get; set; }
        [MaxLength(10)]
        public string AccountType { get; set; }
        public string ProviderProfileUrl { get; set; }
        public string PhotoUrl { get; set; }
        public int AccessFailedCount { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }

        public Token Token { get; set; }
        public ICollection<Friend> Friends { get; set; }

        //public ICollection<Follow> Followers { get; set; }

        //public ICollection<Follow> Following { get; set; }




        [NotMapped]
        public long? WatchMoviesCount { get; set; }
        [NotMapped]
        public long? WatchedMoviesCount { get; set; }

        [NotMapped]
        public long? FriendsCount { get; set; }
        [NotMapped]
        public long? RoomsCount { get; set; }

        [NotMapped]
        public UserConnectionModel Connection { get; set; }

    }
}