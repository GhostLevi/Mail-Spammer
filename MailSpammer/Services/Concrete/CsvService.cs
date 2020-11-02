using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Model;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class CsvService : ICsvService
    {
        public IObservable<TEntity> GetCollectionFromFile<TEntity, TEntityMapper>(string filePath, int skip)
            where TEntityMapper : ClassMap<TEntity>
        {
            return Observable.Create<TEntity>(
                observer =>
                {
                    try
                    {
                        using (var reader = new StreamReader(filePath))
                        {
                            using (var csv = new CsvReader(reader))
                            {
                                csv.Configuration.Delimiter = ",";
                                csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                                csv.Configuration.RegisterClassMap<TEntityMapper>();

                                var records = csv.GetRecords<TEntity>().Skip(skip);

                                foreach (var record in records)
                                {
                                    observer.OnNext(record);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        AppLogger.Error(e.Message);
                    }
                    finally
                    {
                        observer.OnCompleted();
                    }

                    return Disposable.Empty;
                });
        }
    }
}