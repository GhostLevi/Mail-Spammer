using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Model;
using Services;
using Services.Interface;
using Services.Utils;

namespace App
{
    public class Root
    {
        private readonly Lazy<ICsvService> _backgroundWorker =
                    new Lazy<ICsvService>(AppServiceProvider.Get<ICsvService>());
        
        public void Run()
        {
            var disposable = _backgroundWorker.Value.prepareData().Subscribe(result => Console.WriteLine("Eldo"),
                ((exception) => Console.WriteLine(exception.Message)), () => Console.WriteLine("Completed"));
            
            disposable.Dispose();
        }
    }
}