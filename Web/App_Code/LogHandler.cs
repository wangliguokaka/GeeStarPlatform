using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace GeeStar.Common.Logging
{
    public enum LogLevel
    {
        Trace,
        Info,
        Warning,
        Error,
        Critical
    }

    public class LogHandler<T> where T : LoggingCategory
    {
        public static void LogInfo(string message)
        {
            Write(message, TraceEventType.Information, LogLevel.Info, null, Guid.Empty);
        }

        public static void LogInfo(string message, System.Exception exception)
        {
            Write(message, TraceEventType.Information, LogLevel.Info, exception, Guid.Empty);
        }

        public static void LogWarning(string message)
        {
            Write(message, TraceEventType.Warning, LogLevel.Warning, null, Guid.Empty);
        }

        public static void LogWarning(string message, System.Exception exception)
        {
            Write(message, TraceEventType.Warning, LogLevel.Warning, exception, Guid.Empty);
        }

        public static void LogError(string message)
        {
            Write(message, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);
        }

        public static void LogError(string message, System.Exception exception)
        {
            Write(message, TraceEventType.Error, LogLevel.Error, exception, Guid.Empty);
        }

        public static void LogCritical(string message)
        {
            Write(message, TraceEventType.Critical, LogLevel.Critical, null, Guid.Empty);
        }

        public static void LogCritical(string message, System.Exception exception)
        {
            Write(message, TraceEventType.Critical, LogLevel.Critical, exception, Guid.Empty);
        }

        public static void Trace(string message, Guid traceFlag)
        {
            Write(message, TraceEventType.Information, LogLevel.Trace, null, traceFlag);
        }

        public static void Write(string message, TraceEventType eventType, LogLevel level, System.Exception exception, Guid activityId)
        {
            try
            {

                if (exception != null)
                    message += Environment.NewLine + exception.ToString();

                LogEntry logEntry = new LogEntry();
                logEntry.TimeStamp = DateTime.Now;
                logEntry.Message = message;

                object objInfo = Activator.CreateInstance(typeof(T));
                logEntry.Categories.Clear();
                logEntry.Categories.Add(((LoggingCategory)objInfo).Category);

                logEntry.Priority = (int)level;
                logEntry.Severity = eventType;
                logEntry.Title = activityId.ToString();

                Logger.Write(logEntry);
            }
            catch
            {
               
            }
        }
    }
}
