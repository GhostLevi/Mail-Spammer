using System;
using MailKit.Net.Smtp;
using MimeKit;
using Services.Interface;

namespace Services.Concrete
{
    public class EmailService : IEmailService
    {
        public MimeMessage PrepareEmail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rekiny", "rekinyprogramowania@gmail.com"));
            message.To.Add(new MailboxAddress("Sample Czlowiek", "samplemailkurwazebydonikogonieposzlo@gmail.com"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
            };

            return message;
        }
    }
}