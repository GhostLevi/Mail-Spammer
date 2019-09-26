using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Model;
using Org.BouncyCastle.Asn1.X509.SigI;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class EmailService : IEmailService
    {
        public IObservable<OperationResult> SendEmail(Person personData)
        {
            return Observable.Create<OperationResult>(
                (IObserver<OperationResult> observer) =>
                {
                    try
                    {
                        using (var client = new SmtpClient())
                        {
                            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                            client.Authenticate("rekinyprogramowania@gmail.com", "rekprog12345");

                            client.Send(PrepareEmail(personData));
                            
                            client.Disconnect(true);

                            observer.OnNext(new OperationResult.Success());
                            
                            AppLogger.Information($"Email has been sent to {personData.Email}");
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        AppLogger.Error(e.Message);
                        observer.OnNext(new OperationResult.Failure());
                    }
                    
                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }

        private MimeMessage PrepareEmail(Person personData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rekiny", "rekinyprogramowania@gmail.com"));
            message.To.Add(new MailboxAddress($"{personData.FirstName} {personData.LastName}", personData.Email));

            message.Subject = $"New {personData.CarBrand} car deals!";

            var prefix = personData.Gender is Gender.Male ? "Mr." : "Ms.";

            message.Body = new TextPart("plain")
            {
                Text = $@"Dear {personData.FirstName} {personData.LastName},

Do you know that {personData.CarBrand} has new deals on 2019 car models?

Check it at http://www.{personData.CarBrand}.com

Best regards,
Sales Representative of {personData.CarBrand}"
            };

            return message;
        }
    }
}