using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Models;

namespace MovieUniverse.Abstract.Services
{
    public interface IEmailService
    {
        void SendEmail(EMailModel email);
        
    }
}