using System;
using MailKit.Net.Smtp;
using Services.Interface;

namespace Services.Concrete
{
    public class SpamService : ISpamService
    {
        private readonly Lazy<IEmailService> _emailService =
            new Lazy<IEmailService>(AppServiceProvider.Get<IEmailService>());
        
        public void SendMail()
        {
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.friends.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("joey", "password");

                client.Send(_emailService.Value.PrepareEmail());
                client.Disconnect(true);
            }
        }
    }
}