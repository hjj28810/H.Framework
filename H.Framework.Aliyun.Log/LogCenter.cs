using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Framework.Aliyun.Log
{
    public sealed class LogCenter
    {
        private static readonly Lazy<LogCenter> _lazy = new Lazy<LogCenter>(() => new LogCenter());
        public static LogCenter Instance => _lazy.Value;
        private static List<LogCore> _coreDic;
        private readonly object _locker = new object();

        private LogCenter()
        {
            _coreDic = new List<LogCore>();
        }

        public LogCore CoreInstance(string projectName, string accessKeyId = "", string secretAccessKey = "", string endpoint = "")
        {
            LogCore item = null;
            lock (_locker)
            {
                item = _coreDic.FirstOrDefault(x => x.ProjectName == projectName);
                if (item == null)
                {
                    item = new LogCore(projectName, accessKeyId, secretAccessKey, endpoint);
                    _coreDic.Add(item);
                }
                return item;
            }
        }
    }
}