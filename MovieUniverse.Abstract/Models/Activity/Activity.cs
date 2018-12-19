using System;
using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.Abstract.Models
{
    public abstract class Activity
    {
        public string Command { get; set; }
        public AppUser Owner { get; set; } 
        
        public DateTimeOffset Date { get; set; }
    }
}