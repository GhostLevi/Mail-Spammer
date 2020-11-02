using System;
using System.Collections.Generic;
using CsvHelper.Configuration;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface ICsvService
    {
        IObservable<TEntity> GetCollectionFromFile<TEntity, TEntityMapper>(string filePath, int skip)
            where TEntityMapper : ClassMap<TEntity>;
    }
}