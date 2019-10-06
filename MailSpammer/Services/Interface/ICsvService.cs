using System;
using System.Collections.Generic;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface ICsvService
    {
        IObservable<Person> GetCollectionFromFile(string filePath, int skip);
    }
}