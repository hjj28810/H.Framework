using System;
using System.Diagnostics;

namespace H.Framework.Core.Utilities
{
    public static class TimeHelper
    {
        static TimeHelper()
        {
            _watch = new Stopwatch();
        }

        /// <summary>
        /// 服务器初始化时间
        /// </summary>
        public static DateTime ServerInitTime
        {
            private get => _serverInitTime;
            set
            {
                _serverInitTime = value;
                _watch.Restart();
            }
        }

        /// <summary>
        /// 本地Stopwatch
        /// </summary>
        public static Stopwatch _watch;

        private static DateTime _serverInitTime;

        /// <summary>
        /// 服务器当前时间
        /// </summary>
        public static DateTime CurrentServerTime => ServerInitTime.AddMilliseconds(_watch.ElapsedMilliseconds);
    }
}