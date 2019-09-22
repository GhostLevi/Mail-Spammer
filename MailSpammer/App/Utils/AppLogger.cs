using System;
using Serilog;

namespace App.Utils
{
    public static class AppLogger
    {
        public static void Information(string info)
        {
            Log.Information($":: MAIL SPAMMER INFORMATION :: {info}");
        }

        public static void ErrorEx(Exception ex)
        {
            Log.Error($":: MAIL SPAMMER ERROR :: An {ex.Message} has been thrown ");
        }

        public static void Error(string info)
        {
            Log.Error($":: MAIL SPAMMER ERROR :: {info}");
        }
    }
}