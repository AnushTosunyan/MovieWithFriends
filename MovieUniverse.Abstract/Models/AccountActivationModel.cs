using System.ComponentModel.DataAnnotations;

namespace MovieUniverse.Abstract.Models
{
    public class AccountActivationModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } 
        [Required]
        public string Key { get; set; }
    }
}