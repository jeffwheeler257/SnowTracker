using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowTracker
{
    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

        private static readonly string LogFile = Path.Combine(LogDirectory, "DailyLog");
        
        public static void Log(string message)
        {
            Directory.CreateDirectory(LogDirectory);

            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";

            File.AppendAllText(LogFile, entry);
        }
    }
}