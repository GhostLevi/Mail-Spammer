using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Model;
using Services.Interface;
using Services.Utils;

namespace App
{
    public class BackgroundWorker
    {
        private readonly Lazy<ICsvService> _csvService =
            new Lazy<ICsvService>(AppServiceProvider.Get<ICsvService>());

        private readonly Lazy<IEmailService> _emailService =
            new Lazy<IEmailService>(AppServiceProvider.Get<IEmailService>());

        public void Run()
        {
            var disposable = _csvService.Value.PrepareData()
                .Select(
                    (dataResult) => dataResult is ValueOperationResult<IEnumerable<Person>>.Success success
                        ? _emailService.Value.SendEmail(success.Value.FirstOrDefault())
                        : Observable.Return(new OperationResult.Failure()))
                .Switch()
                .Subscribe();


            Console.WriteLine("Eldo");

            disposable.Dispose();
        }
    }
}