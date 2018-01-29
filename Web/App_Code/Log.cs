using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeeStar.Common.Logging;
using System.Diagnostics;
using System.Configuration;
using GeeStar.Common.Helper;

namespace GeeStar.Workflow.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class Log
    {
        #region 字段与属性

        private static List<LoggingCategory> _Categories = null;

        /// <summary>
        /// 日志输出方式集合
        /// </summary>
        public static List<LoggingCategory> Categories
        {
            set
            {
                Log._Categories  = value;
            }

            get
            {
                if(Log._Categories == null)
                {
                    Log._Categories = new List<LoggingCategory>();
                }

                return Log._Categories;
            }
        }

        #endregion 字段与属性

        #region 构造函数

        static Log()
        {
            string Category = ConfigHelper.ReadConfig("LogCategory");

            if (string.IsNullOrEmpty(Category) == true)
            {
                return;
            }

            string[] categories = Category.Split(new char[] { ',' });

            for (int i = 0; i < categories.Length; i++)
            {
                if (categories[i].Trim() == string.Empty)
                {
                    continue;
                }

                System.Reflection.Assembly logAssembly = typeof(LoggingCategory).Assembly;
                Log.Categories.Add(logAssembly.CreateInstance("GeeStar.Common.Logging." + categories[i]) as LoggingCategory);
            }


        }

        #endregion 构造函数

        #region 方法

        public static void LogInfo(string message)
        {
            Log.Write(message, TraceEventType.Information, LogLevel.Info, null, Guid.Empty);
        }

        public static void LogInfo(string message, System.Exception exception)
        {
            Log.Write(message, TraceEventType.Information, LogLevel.Info, exception, Guid.Empty);
        }

        public static void LogWarning(string message)
        {
            Log.Write(message, TraceEventType.Warning, LogLevel.Warning, null, Guid.Empty);
        }

        public static void LogWarning(string message, System.Exception exception)
        {
            Log.Write(message, TraceEventType.Warning, LogLevel.Warning, exception, Guid.Empty);
        }

        public static void LogError(string message)
        {
            Log.Write(message, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);
        }

        public static void LogError(System.Exception ex)
        {
            if (ex != null)
            {
                Log.Write(ex.Message, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);
                Log.Write(ex.StackTrace, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);

                if (ex.InnerException != null)
                {
                    Log.Write(ex.InnerException.Message, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);
                    Log.Write(ex.InnerException.StackTrace, TraceEventType.Error, LogLevel.Error, null, Guid.Empty);
                }
            }
        }

        public static void LogError(string message, System.Exception exception)
        {
            Log.Write(message, TraceEventType.Error, LogLevel.Error, exception, Guid.Empty);
        }

        public static void LogCritical(string message)
        {
            Log.Write(message, TraceEventType.Critical, LogLevel.Critical, null, Guid.Empty);
        }

        public static void LogCritical(string message, System.Exception exception)
        {
            Log.Write(message, TraceEventType.Critical, LogLevel.Critical, exception, Guid.Empty);
        }

        public static void Trace(string message, Guid traceFlag)
        {
            Log.Write(message, TraceEventType.Information, LogLevel.Trace, null, traceFlag);
        }

        public static void Write(string message, TraceEventType eventType, LogLevel level, System.Exception exception, Guid activityId)
        {
            if (Log._Categories == null)
            {
                return;
            }

            for (int i = 0; i < Log._Categories.Count; i++)
            {
                if (Log._Categories[i].GetType() == typeof(DBLogging))
                {
                    LogHandler<DBLogging>.Write(message, eventType, level, exception, activityId);
                }
                else if (Log._Categories[i].GetType() == typeof(TxtLogging))
                {
                    LogHandler<TxtLogging>.Write(message, eventType, level, exception, activityId);
                }
                else if (Log._Categories[i].GetType() == typeof(EventLogging))
                {
                    LogHandler<EventLogging>.Write(message, eventType, level, exception, activityId);
                }               
            }            
        }

        #endregion 方法

    }
}
