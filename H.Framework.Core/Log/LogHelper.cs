using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H.Framework.Core.Log
{
    public static class LogHelper
    {
        private static readonly object _locker = new object();
        private static readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);

        public static void Register(ILogger logger, string name)
        {
            lock (_locker)
            {
                if (!_loggers.ContainsKey(name))
                    _loggers.Add(name, logger);
            }
        }

        public static ILogger GetLogger(string name)
        {
            lock (_locker)
            {
                return _loggers.TryGetValue(name, out ILogger result) ? result : null;
            }
        }

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="logType"></param>
        /// <param name="innnerException"></param>
        public static void WriteLogFile<T>(LogMessage<T> input, LogType logType, Exception innnerException = null)
        {
            GetLogger(Enum.GetName(typeof(LogType), logType)).Info(input, innnerException);
        }

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="logType"></param>
        /// <param name="innnerException"></param>
        public static void WriteLogFile<T>(T input, LogType logType, Exception innnerException = null)
        {
            GetLogger(Enum.GetName(typeof(LogType), logType)).Info(input, innnerException);
        }

        ///// <summary>
        ///// 写入日志文件
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="logType"></param>
        ///// <param name="innnerException"></param>
        //public static void WriteLogFile(string input, LogType logType, Exception innnerException = null)
        //{
        //    WriteLogFile(input, logType, innnerException);
        //}

        public static void WriteLogFile(string title, object data, LogType logType, Exception innnerException = null)
        {
            WriteLogFile(new LogMessage<object> { Title = title, Data = data }, logType, innnerException);
        }

        public static Task WriteLogFileAsync<T>(LogMessage<T> input, LogType logType, Exception innnerException = null)
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteLogFile(input, logType, innnerException);
                }
            });
        }

        public static Task WriteLogFileAsync<T>(T input, LogType logType, Exception innnerException = null)
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteLogFile(input, logType, innnerException);
                }
            });
        }
    }
}