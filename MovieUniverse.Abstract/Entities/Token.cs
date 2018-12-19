using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieUniverse.Abstract.Entities.UserEntityes
{
    public class Token
    {
        [Index(IsUnique = true), MaxLength(35)]
        public string Key { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        [Key, ForeignKey("User")]
        public long UserId { get; set; }

        public AppUser User { get; set; }
    }
}