using System.ComponentModel.DataAnnotations;

namespace MovieUniverse.Abstract.ApiModels
{
    public class UserEditModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public string PhotoUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        public string NewPasswordConfirm { get; set; }
    }
}