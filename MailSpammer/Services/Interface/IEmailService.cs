using System;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface IEmailService
    {
        IObservable<OperationResult> SendEmail(Person personData);
    }
}