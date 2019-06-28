using System;
using System.Diagnostics;

namespace H.Framework.Core.Utilities
{
    public static class TimeHelper
    {
        static TimeHelper()
        {
            Watch = new Stopwatch();
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
                Watch.Restart();
            }
        }

        /// <summary>
        /// 本地Stopwatch
        /// </summary>
        public static Stopwatch Watch { get; }

        private static DateTime _serverInitTime;

        /// <summary>
        /// 服务器当前时间
        /// </summary>
        public static DateTime CurrentServerTime => ServerInitTime.AddMilliseconds(Watch.ElapsedMilliseconds);

        public static long UtcSeconds()
        {
            return UtcSeconds(DateTime.UtcNow);
        }

        public static long UtcSeconds(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}