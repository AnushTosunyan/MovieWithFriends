using System.ComponentModel.DataAnnotations;

namespace MovieUniverse.Abstract.Models
{
    public class OAuthUserLoginModel
    {
        public string Email { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string ProviderId { get; set; }
        [Required]
        public string Name { get; set; }

        public string ProviderProfileUrl { get; set; }
        public string ProviderProfilePhotoUrl { get; set; }

    }
}