using System;

namespace AutomationUtils.Utils
{
    public static class Logger
    {
        public static void Write(string text, LogLevel logLvl = LogLevel.None)
        {
            Console.WriteLine($"{LogLevelInfo(logLvl)}{text}");
        }

        public static void Write(Exception exception, LogLevel logLvl = LogLevel.None)
        {
            Write(exception.ToString(), logLvl);
        }

        public static void Write(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        private static string LogLevelInfo(LogLevel logLvl)
        {
            switch (logLvl)
            {
                case LogLevel.None:
                    return String.Empty;
                case LogLevel.Info:
                    return $"{DateTime.Now} - INFO - ";
                case LogLevel.Warning:
                    return $"{DateTime.Now} - WARNING - ";
                case LogLevel.Error:
                    return $"{DateTime.Now} - ERROR - ";
                default:
                    return string.Empty;
            }
        }

        public enum LogLevel
        {
            None,
            Info,
            Warning,
            Error
        }
    }
}
