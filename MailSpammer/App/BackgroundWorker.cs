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
        private readonly ICsvService _csvService;
        private readonly ISmtpService _smtpService;
        private readonly IWritableOptions<SchedulerConfig> _schedulerConfig;

        public BackgroundWorker(ICsvService csvService, ISmtpService smtpService,
            IWritableOptions<SchedulerConfig> schedulerConfig)
        {
            _csvService = csvService;
            _smtpService = smtpService;
            _schedulerConfig = schedulerConfig;
        }

        public void Run()
        {
            var disposable =
                _csvService.GetCollectionFromFile(@"database.csv")
                    .Skip(_schedulerConfig.Value.StartingPoint)
                    .Buffer(_schedulerConfig.Value.PackageSize)
                    .Select(people =>
                    {
                        var timer = Observable.Timer(TimeSpan.FromSeconds(_schedulerConfig.Value.TimeLimit));

                        var sending = people.Select(person => _smtpService.SendEmail(person)).Concat();

                        return timer.Zip(sending, (time, result) => new OperationResult.Success())
                            .Do(success => { }, (throwable) => { },
                                () => { _schedulerConfig.Update(config => { config.StartingPoint += _schedulerConfig.Value.PackageSize; }); });
                    })
                    .Concat()
                    .RepeatWhen(observable => observable.Delay(TimeSpan.FromSeconds(10)));

            using (disposable.Subscribe(Console.WriteLine))
            {
                Console.Read();
            }
        }
    }
}