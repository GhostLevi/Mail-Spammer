using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public IObservable<ValueOperationResult<IEnumerable<Person>>> GetCollection()
        {
            return Observable.Create<ValueOperationResult<IEnumerable<Person>>>(
                observer =>
                {
                    try
                    {
                        var records = GetRecords(@"database.csv");
                        observer.OnNext(new ValueOperationResult<IEnumerable<Person>>.Success(records));
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
                return csv.GetRecords<Person>();
            }
        }
    }
}