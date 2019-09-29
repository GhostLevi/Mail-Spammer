using System;
using System.IO;
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
                .AddJsonFile("appsettings.json", optional: true);
            var config = configBuilder.Build();
            
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISmtpService, SmtpService>()
                .AddTransient<ICsvService,CsvService>()
                .AddSingleton<EmailGenerator>()
                .Configure<SmtpConfig>(config.GetSection("smtpConfig"))
                .AddSingleton<BackgroundWorker>()
                .BuildServiceProvider();
            
            
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(c=>c.File(@"mail-spammer-log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();
            
            AppLogger.Information($"APPLICATION INITIALIZED {DateTime.Now}");

            var worker = serviceProvider.GetService<BackgroundWorker>();
            worker.Run();

            Log.CloseAndFlush();
        }
    }
}