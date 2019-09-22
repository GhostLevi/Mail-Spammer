using Microsoft.Extensions.DependencyInjection;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISpamService, SpamService>()
                .AddSingleton<IBackgroundWorker, BackgroundWorker>()
                .BuildServiceProvider();

            var bgWorker = serviceProvider.GetService<IBackgroundWorker>();
            
            bgWorker.DoWork();
        }
    }
}