using System;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface ISmtpService
    {
        IObservable<OperationResult> SendEmail(Person person);
//        IObservable<OperationResult> SendEmailsPackage(IEnumerable<MimeMessage> emails);

    }
}