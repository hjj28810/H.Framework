using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.Core.Utilities
{
    public class Utility
    {
        public static string ObjectID
        {
            get { return Guid.NewGuid().ToString("N"); }
        }

        public static long UTCSeconds()
        {
            return (DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}