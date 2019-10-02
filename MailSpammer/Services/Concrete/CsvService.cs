using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CsvHelper;
using Model;
using Services.Interface;
using Services.Utils;

namespace Services.Concrete
{
    public class CsvService : ICsvService
    {
        public IObservable<Person> GetCollectionFromFile(string filePath)
        {
            return Observable.Create<Person>(
                observer =>
                {
                    try
                    {
                        using (var reader = new StreamReader(filePath))
                        using (var csv = new CsvReader(reader))
                        {
                            csv.Configuration.Delimiter = ",";
                            csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                            csv.Configuration.RegisterClassMap<CsvPersonMapper>();
                            var records = csv.GetRecords<Person>();
                            foreach (var person in records)
                            {
                                observer.OnNext(person);
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

        private IEnumerable<Person> GetRecords(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = ",";
                csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                csv.Configuration.RegisterClassMap<CsvPersonMapper>();
                var collection = csv.GetRecords<Person>();
                foreach (var item in collection)
                {
                    Console.WriteLine(item);
                }
                return csv.GetRecords<Person>();
            }
        }
    }
}