using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using Services.Interface;
using Services.Utils;

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

                client.Connect ("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                client.Authenticate("rekinyprogramowania@gmail.com", "rekprog12345");

                client.Send(_emailService.Value.PrepareEmail());
                client.Disconnect(true);
            }
        }
    }
}