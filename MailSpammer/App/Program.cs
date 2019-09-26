using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services;
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
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File($"logs\\mail-spammer-log-{DateTime.Now}.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            AppServiceProvider.Init(serviceProvider);

            AppLogger.Information("APPLICATION INITIALIZED");

            var worker = new BackgroundWorker();
            worker.Run();

            Console.WriteLine();
        }
    }
}