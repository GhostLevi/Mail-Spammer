using System;
using System.Collections.Generic;
using App.Utils;
using Model;
using Services.Utils;

namespace Services.Interface
{
    public interface ICsvService
    {
        IObservable<ValueOperationResult<IEnumerable<Person>>> prepareData();
    }
}