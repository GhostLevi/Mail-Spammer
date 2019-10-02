using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
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
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"config/appsettings.json", optional: true, reloadOnChange: true);
            var config = configBuilder.Build();
            var serviceProvider = new ServiceCollection()
                .ConfigureWritable<SchedulerConfig>(config,"schedulerConfig")
                .Configure<SmtpConfig>(config.GetSection("smtpConfig"))
                .AddSingleton<ISmtpService, SmtpService>()
                .AddSingleton<ICsvService, CsvService>()
                .AddSingleton<IEmailGenerator, EmailGenerator>()
                .AddSingleton<BackgroundWorker>()
                .BuildServiceProvider();


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(c => c.File(@"mail-spammer-log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();

            AppLogger.Information($"APPLICATION INITIALIZED {DateTime.Now}");

            var worker = serviceProvider.GetService<BackgroundWorker>();
            worker.Run();

            Log.CloseAndFlush();
        }
    }
}