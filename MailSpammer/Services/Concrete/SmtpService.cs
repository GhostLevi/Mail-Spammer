using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Model;
using Org.BouncyCastle.Asn1.X509.SigI;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class SmtpService : ISmtpService
    {
        private readonly SmtpConfig _smtpConfig;

        public SmtpService(IOptions<SmtpConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        
        public IObservable<OperationResult> SendEmail(MimeMessage email)
        {
            return Observable.Create<OperationResult>(
                observer =>
                {
                    try
                    {
                        using (var client = new SmtpClient())
                        {
//                             For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            client.Connect(_smtpConfig.Host, _smtpConfig.Port, true);

                            client.Authenticate(_smtpConfig.Username, _smtpConfig.Password);

                            client.Send(email);

                            client.Disconnect(true);

                            observer.OnNext(new OperationResult.Success());

                            AppLogger.Information($"Email has been sent to {email.To.Mailboxes.FirstOrDefault()?.Address}");
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

        public IObservable<OperationResult> SendEmailsPackage(IEnumerable<MimeMessage> emails)
        {
            return Observable.Create<OperationResult>(
                observer =>
                {
                    try
                    {
                        using (var client = new SmtpClient())
                        {
//                             For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            client.Connect(_smtpConfig.Host, _smtpConfig.Port, true);

                            client.Authenticate(_smtpConfig.Username, _smtpConfig.Password);

                            foreach (var email in emails)
                            {
                                client.Send(email);
                            }

                            client.Disconnect(true);

                            observer.OnNext(new OperationResult.Success());

                            AppLogger.Information($"Emails has been sent.");
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

        
    }
}