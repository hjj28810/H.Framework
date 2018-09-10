using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;

//[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension = "log4net", Watch = true)]

namespace H.Framework.Core.Log
{
    public class Log4NetLogger : ILogger
    {
        static Log4NetLogger()
        {
            _repository = LogManager.CreateRepository("NETStandardRepository");
            log4net.Config.XmlConfigurator.Configure(_repository, new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
        }

        private static readonly Dictionary<string, ILog> LoggerDictionary = new Dictionary<string, ILog>();
        private readonly ILog _logger;
        private object _locker = new object();
        private static readonly ILoggerRepository _repository;

        public Log4NetLogger(string logType)
        {
            if (!LoggerDictionary.ContainsKey(logType))
            {
                lock (_locker)
                {
                    if (!LoggerDictionary.ContainsKey(logType))
                    {
                        LoggerDictionary.Add(logType, LogManager.GetLogger(_repository.Name, logType));
                    }
                }
            }
            _logger = LoggerDictionary[logType];
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        #region sync

        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        public void Debug(object message, Exception ex)
        {
            _logger.Debug(message, ex);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            _logger.DebugFormat(format, arg0);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            _logger.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.DebugFormat(format, arg0, arg1, arg2);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            _logger.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            _logger.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(object message, Exception ex)
        {
            _logger.Fatal(message, ex);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _logger.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            _logger.FatalFormat(format, arg0);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            _logger.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.FatalFormat(format, arg0, arg1, arg2);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception ex)
        {
            _logger.Info(message, ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            _logger.InfoFormat(format, arg0);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            _logger.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.InfoFormat(format, arg0, arg1, arg2);
        }

        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        public void Warn(object message, Exception ex)
        {
            _logger.Warn(message, ex);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            _logger.WarnFormat(format, arg0);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            _logger.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.WarnFormat(format, arg0, arg1, arg2);
        }

        #endregion sync
    }
}