using System;
using System.Collections.Generic;

namespace Services.Utils
{
    public static class Helpers
    {
        public static IEnumerable<List<T>> splitList<T>(List<T> input, int nSize)  
        {        
            for (var i=0; i < input.Count; i+= nSize) 
            { 
                yield return input.GetRange(i, Math.Min(nSize, input.Count - i)); 
            }  
        } 
    }
}