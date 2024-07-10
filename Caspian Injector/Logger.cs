using Caspian_Injector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caspian
{
    public enum LogLevel
    {
        Info, Warn, Error
    }
    public class Logger
    {
        private static readonly string logFilePath = $"{System.IO.Path.GetTempPath()}\\log.txt";

        public static void log(String message, LogLevel id, String worker)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string logMessage = $"[{time}] [{id.ToString()}] [{worker}] {message}";

            switch (id)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            WriteLogToFile(logMessage);

            App.Current.Dispatcher.Invoke(() =>
            {
                MainWindow._statusTextBlock.Text = $"[{id.ToString()}] [{worker}] {message}";
            });
        }

        public static void ClearLogFile()
        {
            System.IO.File.WriteAllText(logFilePath, string.Empty);

        }

        public static void WriteLogToFile(string logMessage)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
