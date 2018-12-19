using System;
using System.Net;
using System.Net.Mail;
using MovieUniverse.Abstract.Models;
using MovieUniverse.Abstract.Services;

namespace MovieUniverse.Services.ServiceImpl
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EMailModel email)
        {
            
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("hovo563@gmail.com");
                mail.To.Add(email.DestinationAddress);
                mail.Subject = email.Subject;
                mail.Body = email.MessegeBody;
                mail.IsBodyHtml = true;
                
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                try
                {
                    smtp.Credentials = new NetworkCredential("hovo563@gmail.com", "hrashq56$#");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    int b = 5;
                }
            }

        }
    }
}