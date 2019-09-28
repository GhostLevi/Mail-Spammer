using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
                .Select((job) =>
                {
                    if (job is ValueOperationResult<IEnumerable<Person>>.Success list)
                    {
                        return list.Value.ToList().ToObservable().Buffer(100)
                            .Select(people =>
                            {
                                var timer = Observable.Timer(TimeSpan.FromSeconds(5));

                                var sending = people.Select(person => _emailService.Value.SendEmail(person)).Concat();

                                return timer.Zip(sending, (time, result) => { return new OperationResult.Success(); });

                                //return people.Select(person => _emailService.Value.SendEmail(person))
                                //    .Concat().Concat(Observable.Timer(TimeSpan.FromSeconds(5)).Select(x=>new OperationResult.Success()));
                            }).Switch();
                    }
                    return Observable.Return(new OperationResult.Failure() as OperationResult);
                    
                }).Switch().Subscribe();


//            var disposable = _csvService.Value.PrepareData()
//                .Select(
//                    (dataResult) => dataResult is ValueOperationResult<IEnumerable<Person>>.Success success
//                        ? _emailService.Value.SendEmail(success.Value.FirstOrDefault())
//                        : Observable.Return(new OperationResult.Failure()))
//                .Switch()
//                .Subscribe();


            Console.WriteLine("Eldo");
        }
    }
}