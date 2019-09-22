﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using Model;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class CsvService : ICsvService
    {
        public IObservable<ValueOperationResult<IEnumerable<Person>>> prepareData()
        {
            return Observable.Create<ValueOperationResult<IEnumerable<Person>>>(
                (IObserver<ValueOperationResult<IEnumerable<Person>>> observer) =>
                {
                    try
                    {
                        using (var reader = new StreamReader(@"../../../database.csv"))
                        using (var csv = new CsvReader(reader))
                        {
                            csv.Configuration.Delimiter = ",";
                            csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                            csv.Configuration.RegisterClassMap<CsvPersonMapper>();
                            var records = csv.GetRecords<Person>().ToList();
                            observer.OnNext(new ValueOperationResult<IEnumerable<Person>>.Success(records));
                            observer.OnCompleted();
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        AppLogger.Error(e.Message);
                    }
                    
                    return Disposable.Empty;
                });
        }
    }
}