using System;

namespace H.Framework.Core.Utilities
{
    public class Utility
    {
        public static string ObjectID => Guid.NewGuid().ToString("N");

        public static long UTCSeconds()
        {
            return (DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}