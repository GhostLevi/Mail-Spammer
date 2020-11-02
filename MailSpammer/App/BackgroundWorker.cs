using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
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
            var disposable = Observable.Defer(() =>
            {
                AppLogger.Information("Checking for new data...");

                return _csvService
                    .GetCollectionFromFile<Person, CsvPersonMapper>(@"database.csv",
                        _schedulerConfig.Value.StartingPoint)
                    .Buffer(_schedulerConfig.Value.PackageSize)
                    .Select(people =>
                    {
                        var timer = Observable.Timer(TimeSpan.FromSeconds(_schedulerConfig.Value.TimeLimit))
                            .DoOnComplete(() => { AppLogger.Information($"Time limit have passed."); });

                        var sending = people.Select(person => _smtpService.SendEmail(person))
                            .Concat()
                            .DoOnNext(_ => { _schedulerConfig.Update(config => { config.StartingPoint += 1; }); })
                            .Buffer(people.Count)
                            .DoOnComplete(() => { AppLogger.Information($"Package sent."); });

                        return timer.Zip(sending, (time, result) => new OperationResult.Success())
                            .DoOnComplete(() =>
                            {
                                AppLogger.Information(
                                    $"{_schedulerConfig.Value.PackageSize} mails sent in max of {_schedulerConfig.Value.TimeLimit} seconds.");
                            });
                    })
                    .Concat();
            }).RepeatWhen(observable => observable.Delay(TimeSpan.FromSeconds(3)));

            using (disposable.Subscribe())
            {
                Console.Read();
            }
        }
    }
}