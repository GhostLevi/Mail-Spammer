using System;
using System.Collections.Generic;
using MimeKit;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface ISmtpService
    {
        IObservable<OperationResult> SendEmail(MimeMessage email);
        IObservable<OperationResult> SendEmailsPackage(IEnumerable<MimeMessage> emails);

    }
}