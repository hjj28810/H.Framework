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
            return DateTime.UtcNow.ToLong();
        }

        //public static long UtcSeconds(this DateTime time)
        //{
        //    return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        //}
        
        public static DateTime ToDateTime(this long d, bool isMillisecond = false)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var resultTime = isMillisecond ? start.AddMilliseconds(d) : start.AddSeconds(d);
            return resultTime.AddHours(8);
        }

        public static long ToLong(this DateTime dt, bool isMillisecond = false)
        {
            //var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //var toNow = dt.Subtract(dtStart);
            //var timeStamp = toNow.Ticks;
            //var len = isMillisecond ? 4 : 7;
            //timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - len));
            //return timeStamp;
            var jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var result = dt.AddHours(-8) - jan1st1970;
            if (isMillisecond)
                return (long)result.TotalMilliseconds;
            else
                return (long)result.TotalSeconds;
        }

    }
}