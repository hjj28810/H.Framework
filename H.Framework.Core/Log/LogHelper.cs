using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H.Framework.Core.Log
{
    public static class LogHelper
    {
        private static readonly object _locker = new object();
        private static Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);

        public static void Register(ILogger logger, string name)
        {
            lock (_locker)
            {
                if (!loggers.ContainsKey(name))
                    loggers.Add(name, logger);
            }
        }

        public static ILogger GetLogger(string name)
        {
            lock (_locker)
            {
                return loggers.TryGetValue(name, out ILogger result) ? result : null;
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
        public static void WriteLogFile(string input, LogType logType, Exception innnerException = null)
        {
            WriteLogFile(new LogMessage<object> { Title = input }, logType, innnerException);
        }

        public static void WriteLogFile(string input, object data, LogType logType, Exception innnerException = null)
        {
            WriteLogFile(new LogMessage<object> { Title = input, Data = data }, logType, innnerException);
        }

        public static void WriteLogFileAsync<T>(LogMessage<T> input, LogType logType, Exception innnerException = null)
        {
            lock (_locker)
            {
                Task.Run(() =>
                {
                    WriteLogFile(input, logType, innnerException);
                });
            }
        }
    }
}