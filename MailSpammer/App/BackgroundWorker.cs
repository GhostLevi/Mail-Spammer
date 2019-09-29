using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CsvHelper;
using Model;
using Services.Interface;
using Services.Utils;

namespace App
{
    public class BackgroundWorker
    {
        private readonly Lazy<ICsvService> _csvService =
            new Lazy<ICsvService>(AppServiceProvider.Get<ICsvService>());

        private readonly Lazy<ISmtpService> _emailService =
            new Lazy<ISmtpService>(AppServiceProvider.Get<ISmtpService>());

        public void Run()
        {
            var disposable = _csvService.Value.PrepareData()
                .Select((job) =>
                {
                    if (job is ValueOperationResult<IEnumerable<Person>>.Success list)
                    {
                        return list.Value.ToList().ToObservable().Buffer(10)
                            .Select(people =>
                            {
                                var timer = Observable.Timer(TimeSpan.FromSeconds(20));

                                var sending = people.Select(person =>
                                        _emailService.Value.SendEmailsPackage(person).SubscribeOn(NewThreadScheduler.Default))
                                    .Concat();

                                return timer.Zip(sending, (time, result) => new OperationResult.Success());
                            }).Concat();
                    }

                    return Observable.Return(new OperationResult.Failure() as OperationResult);
                }).Switch().Repeat();

            using (var handle = disposable.Subscribe(Console.WriteLine))
            {
                Console.Read();
            }
        }
    }
}