using MovieUniverse.Abstract.Entities;

namespace MovieUniverse.Abstract.Models
{
    public class UserMovieActivity:Activity
    {
         public Movie Subject { get; set; }
        public int? Rating { get; set; }
    }
}