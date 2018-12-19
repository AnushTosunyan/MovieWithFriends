using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.Abstract.Models
{
    public class PrivateUserModel
    {
         public string Email { get; set; }
         public Token Token { get; set; }
    }
}