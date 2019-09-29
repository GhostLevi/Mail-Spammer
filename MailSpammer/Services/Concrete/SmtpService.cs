using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Model;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class SmtpService : ISmtpService
    {
        private readonly IEmailGenerator _emailGenerator;

        public SmtpService(IEmailGenerator emailGenerator)
        {
            _emailGenerator = emailGenerator;
        }

        public IObservable<OperationResult> SendEmail(Person personData)
        {
            return Observable.Create<OperationResult>(
                observer =>
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            var mail = await _emailGenerator.GenerateEmail(personData);

                            observer.OnNext(new OperationResult.Success());

                            AppLogger.Information(
                                $"Email has been sent to {mail.Data.ToAddresses.FirstOrDefault()}");
                            
                            await mail.SendAsync();
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            AppLogger.Error(e.Message);
                            observer.OnNext(new OperationResult.Failure());
                        }
                    });

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }
    }
}