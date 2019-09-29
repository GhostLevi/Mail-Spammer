using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using CsvHelper;
using Model;
using Services.Interface;
using Services.Utils;

namespace App
{
    public class BackgroundWorker
    {
        private readonly ICsvService _csvService;
        private readonly IEmailService _emailService;

        public BackgroundWorker(ICsvService csvService, IEmailService emailService)
        {
            _csvService = csvService;
            _emailService = emailService;
        }

        public void Run()
        {
            var disposable = _csvService.GetCollection()
                .Select(job =>
                {
                    if (job is ValueOperationResult<IEnumerable<Person>>.Success list)
                    {
                        return list.Value.ToList().ToObservable().Buffer(100)
                            .Select(people =>
                            {
                                var timer = Observable.Timer(TimeSpan.FromSeconds(60));

                                var sending = people.Select(person => _emailService.SendEmail(person)).Concat();

                                return timer.Zip(sending, (time, result) => new OperationResult.Success());
                                
                            }).Concat();
                    }

                    return Observable.Return(new OperationResult.Failure() as OperationResult);
                    
                }).Switch().Repeat();

            using (disposable.Subscribe(Console.WriteLine))
            {
                Console.Read();
            }
        }
    }
}