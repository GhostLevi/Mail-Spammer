﻿using System;
using App.Interfaces;
using App.Utils;

namespace App
{
    public class Root
    {
        private readonly Lazy<IBackgroundWorker> _backgroundWorker =
                    new Lazy<IBackgroundWorker>(AppServiceProvider.Get<IBackgroundWorker>());
        
        public void Run()
        {
            _backgroundWorker.Value.DoWork();
        }
    }
}