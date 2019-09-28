using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services.Concrete;
using Services.Interface;
using Services.Utils;

namespace App
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IEmailService, EmailService>()
                .AddTransient<ICsvService,CsvService>()
                .BuildServiceProvider();
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(c=>c.File(@"mail-spammer-log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();
            
            AppServiceProvider.Init(serviceProvider);

            AppLogger.Information($"APPLICATION INITIALIZED {DateTime.Now}");

            var worker = new BackgroundWorker();
            worker.Run();

            Log.CloseAndFlush();
        }
    }
}