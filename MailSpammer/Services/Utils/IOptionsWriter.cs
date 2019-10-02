using System;
using Model;
using Newtonsoft.Json.Linq;

namespace Services.Utils
{
    interface IOptionsWriter
    {
        void UpdateOptions(Action<JObject> callback, bool reload = true);
    }
}