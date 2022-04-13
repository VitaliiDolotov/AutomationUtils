using System;
using System.Diagnostics;

namespace AutomationUtils.Utils
{
    public class TimeLogger : IDisposable
    {
        private readonly string _message;
        /// <summary>
        /// Request execution timeout in milliseconds
        /// </summary>
        private readonly int _timeout;
        private readonly Stopwatch _stopwatch;
        private readonly Logger.LogLevel _logLevel;

        public TimeLogger(string message, int timeout)
        {
            _message = message;
            _timeout = timeout;
            _stopwatch = Stopwatch.StartNew();
        }

        public TimeLogger(string message, int timeout, Logger.LogLevel logLevel) : this(message, timeout)
        {
            _logLevel = logLevel;
        }

        public static IDisposable LogDuration(string message, int timeout)
        {
            return new TimeLogger(message, timeout);
        }

        public static IDisposable LogDuration(string message, int timeout, Logger.LogLevel logLevel)
        {
            return new TimeLogger(message, timeout, logLevel);
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var duration = _stopwatch.ElapsedMilliseconds;
            ValidateDuration(duration);
        }

        private void ValidateDuration(long duration)
        {
            var isExecutedInTime = duration <= _timeout;

            var logMessage = isExecutedInTime
                ? $"{_message} completed in {duration}ms."
                : $"{_message} took more than {_timeout} milliseconds. Actual duration: {duration}ms.";

            // For log level 'Error' we must throw exception
            if (_logLevel.Equals(Logger.LogLevel.Error))
            {
                // Throw if request not executed in time
                if (!isExecutedInTime)
                {
                    throw new Exception(logMessage);
                }

                // Just log execution time to have it in logs
                Logger.Write(logMessage, _logLevel == Logger.LogLevel.Info);
                return;
            }

            Logger.Write(logMessage, _logLevel);
        }
    }
}
