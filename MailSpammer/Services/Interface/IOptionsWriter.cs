using System;
using Newtonsoft.Json.Linq;

namespace Services.Interface
{
    interface IOptionsWriter
    {
        void UpdateOptions(Action<JObject> callback, bool reload = true);
    }
}