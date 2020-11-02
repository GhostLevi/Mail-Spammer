using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;
using Model;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class SmtpService : ISmtpService
    {
        private readonly IEmailGenerator _emailGenerator;
        private readonly SmtpConfig _smtpConfig;

        public SmtpService(IOptions<SmtpConfig> smtpConfig, IEmailGenerator emailGenerator)
        {
            _smtpConfig = smtpConfig.Value;
            _emailGenerator = emailGenerator;
        }

        public IObservable<OperationResult> SendEmail(Person personData)
        {
            return Observable.Create<OperationResult>(
                observer =>
                {
                    return Task.Run(async () =>
                    {
                        try
                        {
                            using (var smtpClient = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port)
                            {
                                EnableSsl = true,
                                Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password)
                            })
                            {
                                var mail = await _emailGenerator.GenerateEmail(personData);

                                var stopWatch = Stopwatch.StartNew();

                                await smtpClient.SendMailAsync(mail);

                                observer.OnNext(new OperationResult.Success());

                                AppLogger.Debug(
                                    $"Id : {personData.Id} - Email has been sent to {personData.Email} in {stopWatch.Elapsed.TotalSeconds:0.000} seconds.");
                            }
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            AppLogger.Error(e.Message);
                        }

                        observer.OnCompleted();
                        return Disposable.Empty;
                    });
                });
        }
    }
}