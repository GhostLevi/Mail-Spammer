using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services;
using Services.Concrete;
using Services.Interface;

namespace App
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISpamService, SpamService>()
                .AddSingleton<IBackgroundWorker, BackgroundWorker>()
                .AddSingleton<IEmailService, EmailService>()
                .BuildServiceProvider();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File($"logs\\mail-spammer-log-{DateTime.Now}.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            AppServiceProvider.Init(serviceProvider);

            AppLogger.Information("APPLICATION INITIALIZED");

            var root = new Root();
            root.Run();
        }
    }
}