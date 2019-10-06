using System;
using Serilog;

namespace Services.Utils
{
    public static class AppLogger
    {
        public static void Information(string info)
        {
            Log.Information($"{info}");
        }

        public static void Debug(string text)
        {
            Log.Debug($"{text}");
        }
        
        public static void ErrorEx(Exception ex)
        {
            Log.Error($"An {ex.Message} has been thrown ");
        }

        public static void Error(string text)
        {
            Log.Error($"{text}");
        }
    }
}